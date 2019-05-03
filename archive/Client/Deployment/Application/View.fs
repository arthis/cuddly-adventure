module Livingstone.Deployment.Application.View


open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma

open Livingstone.Deployment.Application.Types
open Livingstone.ClientHelpers
open Livingstone.Shared

let showComponent model dispatch=

    
    let isActive = (fun c -> c.application) >> Application.isApplicationOption model.ApplicationToDeploy
    let showValue = Application.getNameFromOptionOrDefault model.ApplicationToDeploy "Choose a Component"  
    let getName = (fun c -> c.application.name) 
        
    match model.Components with
    | Some(components) -> Some ("Component : " , dropdown isActive showValue ChooseComponent components getName dispatch)
    | None -> None


let showVersion model dispatch=

    let isActiveVersion v = 
        match model.versionToDeploy with
        | Some(current) -> current = v
        | None -> false

    let currentValueDropDownList =
        match model.versionToDeploy with
        | Some(current) -> Version.value current
        | None -> "Choose a version"    

    

    match model.versionsAvailableForSelectedComponent with
    | None -> None
    | Some(versions) -> Some ("Version : " , dropdown isActiveVersion currentValueDropDownList ChooseVersion versions Version.value dispatch)


let showTargetEnv model dispatch=
    match model.versionToDeploy, model.ApplicationToDeploy with
    | Some x, Some y -> 
         Some   ("targetEnv : " , input
            [ 
                Value (model.namespaceToDeployTo)
                OnChange (fun ev -> 
                    printf "evt value : %A" ev.Value
                    ev.Value |> ChooseNamespace |> dispatch  )
            ]
        )
        
    | _ -> None 



let showDeployButton model dispatch =
    match model.versionToDeploy, model.ApplicationToDeploy with
    | Some x, Some y -> Some ("", Livingstone.ClientHelpers.button2 "deploy application" (fun _ -> dispatch DeployApplication))
    | _ -> None




let view (model : Model) (dispatch:DeployApplicationMsg->unit) =

    [
        Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Left) ] ]
            [ 
                Heading.h3 [] [ str ("  Application: ") ] 
                
            ] 

        panelOptions model.deploymentCreated
            (fun a-> Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Left) ] ] [str "deployment Created"])
            (formOptions []
                [
                    showComponent model dispatch
                    showVersion  model dispatch
                    showTargetEnv model dispatch
                    showDeployButton  model dispatch
                ])

        
                
    ]
          

