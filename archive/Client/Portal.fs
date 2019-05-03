module Livingstone.Portal

open Elmish
open Elmish.React

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch
open Fulma
open System.Collections.Generic

open Thoth.Json

open Livingstone.Shared
open Livingstone.ClientHelpers


type PortalView =
| DeploymentView of DeploymentSection.Model


type Model = { 
    CurrentPortal : PortalView option
}


type PortalMsg =
| ShowDeployment
| ShowAdministration
| DeployView of DeploymentSection.DeploymentViewMsg





// defines the initial state and initial command (= side-effect) of the application
let init () : Model * Cmd<PortalMsg> =
    let initialModel = { 
        CurrentPortal = None 
    }
    initialModel, Cmd.none


// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg : PortalMsg) (currentModel : Model) : Model * Cmd<PortalMsg> =
    match  msg with
    | ShowDeployment ->
        let model,subCmd = DeploymentSection.init()
        let nextModel = { 
            currentModel with CurrentPortal = Some <| DeploymentView model
        }
        nextModel, Cmd.map DeployView  subCmd
    | DeployView msg' ->
        let res, cmd =  
            match currentModel.CurrentPortal with
            | Some(DeploymentView(m)) -> DeploymentSection.update msg' m
            | _ -> DeploymentSection.init()
        { currentModel with CurrentPortal = Some(DeploymentView(res)) }, Cmd.map DeployView cmd
    | _ -> currentModel, Cmd.none


let safeComponents =
    [
        // p [] [img [ Src "/Images/livingstone.png"; Style [ Width(100) ] ]]
        // p [ ] [  strong [] [ str "Living Stone" ]]
    ]
    
let showPortal (model : Model) dispatch =
    match model.CurrentPortal with
    | Some ( DeploymentView( m)) -> 
        DeploymentSection.view m ( PortalMsg.DeployView >> dispatch)
    | _ -> []  

let view (model : Model) dispatch =
        div [] [
            Navbar.navbar 
                [ 
                    // Navbar.Color IsPrimary 
                ]
                [ Navbar.Item.div [ ]
                    [ 
                        //Heading.h2 [ ] [ str "Living Stone" ] 
                        span [] [ str "deployment tooling" ] 
                    ]  
                ]

            Columns.columns [ ]
                [ 
                    Column.column [ Column.Option.Width(Screen.Desktop, Column.Is1) ]  [ 
                        button "deployment" (fun _ -> dispatch ShowDeployment) 
                    ]
                    Column.column [] [ 
                        Content.content [] <| showPortal model dispatch                     
                    ]
                ]

            // Footer.footer [ ]
            //     [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
            //         [ Content.content [] safeComponents ] 
            //     ] 
        ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram init update view
#if DEBUG
|> Program.withConsoleTrace
|> Program.withHMR
#endif
|> Program.withReact "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
