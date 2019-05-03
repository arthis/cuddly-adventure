module Livingstone.DeploymentSection


open Elmish
open Elmish.React

open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fable.PowerPack.Fetch

open Thoth.Json

open Livingstone.Shared


open Fulma
open System.Collections.Generic
open ClientHelpers


type DeploymentTypeView =
    | DeployApplicationView
    | DeployProductView
    | DeployAddressingView
    | DeployFullNamespaceView



type Model = { 
    CurrentView : DeploymentTypeView option
    DeploymentApplicationState : Deployment.Application.Types.Model option
    DeploymentProductState : Deployment.Product.Types.Model option

}

type DeploymentViewMsg =
| ShowDeployApplication
| ShowDeployProduct
| ShowDeployAddressing
| ShowDeployFullNamespace
| DeployProduct of Deployment.Product.Types.DeployProductMsg
| DeployApplication of Deployment.Application.Types.DeployApplicationMsg




let init () =
    let initialModel = { 
        CurrentView = None
        DeploymentApplicationState = None
        DeploymentProductState= None
    }

    initialModel,Cmd.none


let update (msg : DeploymentViewMsg) (currentModel : Model) : Model * Cmd<DeploymentViewMsg> = 
    match msg with 
    | ShowDeployApplication ->
        let state, subCmd = Deployment.Application.State.init()
        let nextModel = {  
            currentModel with 
                CurrentView = Some DeployApplicationView ;  
                DeploymentApplicationState= Some(state) ; 
                DeploymentProductState = None;
        }   
        nextModel, Cmd.map DeployApplication subCmd
    | ShowDeployProduct ->
        let state, subCmd = Deployment.Product.State.init()
        let nextModel = { 
            currentModel with 
                CurrentView = Some DeployProductView
                DeploymentApplicationState= None ; 
                DeploymentProductState = Some(state);
        }
        nextModel, Cmd.map DeployProduct subCmd    
    | DeployApplication   cmd  ->
        let newModel,subCmd =
            match currentModel.DeploymentApplicationState with 
            | Some (m) -> Deployment.Application.State.update cmd m
            | None -> Deployment.Application.State.init() 
        { currentModel with  DeploymentApplicationState= Some newModel }, Cmd.map DeployApplication subCmd
    | DeployProduct  cmd  ->
        printf "Deployment DeployProduct %A" cmd
        let newModel,subCmd =
            match currentModel.DeploymentProductState with 
            | Some (m) -> Deployment.Product.State.update cmd m
            | None -> Deployment.Product.State.init() 
        { currentModel with  DeploymentProductState= Some newModel }, Cmd.map DeployProduct subCmd        
    
    | ShowDeployAddressing ->
        let nextModel = { 
            currentModel with CurrentView = Some DeployAddressingView
        }
        nextModel, Cmd.none        
    | ShowDeployFullNamespace ->
        let nextModel = { 
            currentModel with CurrentView = Some DeployFullNamespaceView
        }
        nextModel, Cmd.none            



let showView (model : Model) dispatch =
    match model.CurrentView, model.DeploymentApplicationState, model.DeploymentProductState with
    | Some ( DeployApplicationView), Some(state),_ -> 
        Deployment.Application.View.view state ( DeploymentViewMsg.DeployApplication >> dispatch)
    | Some ( DeployProductView),_ ,Some(state) -> 
        Deployment.Product.View.view state ( DeploymentViewMsg.DeployProduct >> dispatch)        
    | _ -> [] 

let view (model : Model) dispatch =
    [
        Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Left) ] ]
            [ 
                Heading.h3 [] [ str ("Deployments: ") ] 
            ] 

        Columns.columns []
            [ 
                Column.column [] [ button "deploy application" (fun _ -> dispatch ShowDeployApplication) ]
                Column.column [] [ button "deploy product" (fun _ -> dispatch ShowDeployProduct) ]
                Column.column [] [ button "deploy Addressing" (fun _ -> dispatch ShowDeployAddressing) ]
                Column.column [] [ button "deploy FullNamespace" (fun _ -> dispatch ShowDeployFullNamespace) ]
            ] 

        Content.content [] <| showView model dispatch             



    ]