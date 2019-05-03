module Livingstone.Deployment.Product.View


open Fable.Helpers.React
open Fable.Helpers.React.Props
open Fulma

open Livingstone.Shared
open Livingstone.ClientHelpers
open Livingstone.Deployment.Product.Types



let showContainer model dispatch=

    let isActiveProduct (p:ProductVersionReadModel) = 
        match model.productToDeploy with
        | Some(current) -> current.id = p.versionnedProduct.product.id
        | None -> false

    let currentValueDropDownList =
        match model.productToDeploy with
        | Some(product) -> product.name
        | None -> "Choose a product"    
        
    match model.products with
    | Some(productList) -> Some ("Product : " , dropdown isActiveProduct currentValueDropDownList ChooseProduct productList (fun p -> p.versionnedProduct.product.name) dispatch)
    | None -> None


let showVersion model dispatch=

    let isActiveVersion v = 
        match model.versionToDeploy with
        | Some(current) ->  current =  v
        | None -> false

    let currentValueDropDownList =
        match model.versionToDeploy with
        | Some(current) -> Version.value current
        | None -> "Choose a version"    

    

    match model.versionsAvailableForSelectedProduct with
    | None -> None
    | Some(versions) -> Some ("Version : " , dropdown isActiveVersion currentValueDropDownList ChooseVersion versions Version.value dispatch)


let showTargetEnv model dispatch=
    match model.versionToDeploy, model.productToDeploy with
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
    match model.versionToDeploy, model.productToDeploy with
    | Some x, Some y -> Some ("", Livingstone.ClientHelpers.button2 "deploy application" (fun _ -> dispatch DeployProduct))
    | _ -> None




let view (model : Model) (dispatch:DeployProductMsg->unit) =
    [
        Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Left) ] ]
            [ 
                Heading.h3 [] [ str ("Product : ") ] 
                
            ] 

        panelOptions model.deploymentCreated
            (fun a-> Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Left) ] ] [str "deployment Created"])
            (formOptions []
                [
                    showContainer model dispatch
                    showVersion  model dispatch
                    showTargetEnv model dispatch
                    showDeployButton  model dispatch
                ])

        
                
    ]
              