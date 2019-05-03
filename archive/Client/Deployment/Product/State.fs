module Livingstone.Deployment.Product.State


open Elmish
open Livingstone.Shared
open Livingstone.Deployment.Product.Types
open Livingstone.Deployment.Product.Rest



let init () : Model * Cmd<DeployProductMsg> =
    let initialModel = { 
        products = None 
        productToDeploy = None
        namespaceToDeployTo = "ci"
        versionsAvailableForSelectedProduct = None
        versionToDeploy = None
        deploymentCreated = None
    }
    let loadCountCmd =
        Cmd.ofPromise
            initialProduct
            []
            (Ok >> InitialProductLoaded)
            (Error >> InitialProductLoaded)
    initialModel, loadCountCmd


let update (msg : DeployProductMsg) (model : Model) : Model * Cmd<DeployProductMsg> =
    match  msg with
    | InitialProductLoaded (Ok products)->
        let nextModel = { 
            products = Some products;
            productToDeploy = None;
            namespaceToDeployTo = "ci"
            versionsAvailableForSelectedProduct = None
            versionToDeploy = None;
            deploymentCreated = None
        }
        nextModel, Cmd.none
    | InitialProductLoaded (Error ex)->        
        printf "InitialProductLoaded error %A" ex
        model, Cmd.none    
    | DeployProduct ->
        let deployProductCmd =
            match   model.productToDeploy, model.versionToDeploy  with
            | Some (c), Some(v) ->
                     let cmd = { versionnedProduct={ product= c; version=v};  targetEnv = DekraSubscription <| Namespace model.namespaceToDeployTo }                    
                     Cmd.ofPromise
                        (deployProduct cmd)
                        []
                        (Ok >> ProductDeployed)
                        (Error >> ProductDeploymentFailed)
 
            | _ -> failwith "cannot deploy with no Component chosen or version."
        model, deployProductCmd
    | ChooseProduct p->
        printf "product chose %A" p

        let productVersions =
            match model.products with
            | Some(products) ->
                products
                |> Seq.filter (fun p -> p.versionnedProduct.product=p.versionnedProduct.product)
                |> Seq.map (fun p -> p.versionnedProduct.version)
                |> Seq.toArray
            | None -> [||]
        let nextModel = 
            { model with 
                productToDeploy = Some(p.versionnedProduct.product); 
                versionToDeploy = None  
                versionsAvailableForSelectedProduct = Some(productVersions);
                deploymentCreated= None
            }
        nextModel, Cmd.none    
    | ChooseVersion v->
        printf "version chose %A" v
        let nextModel = { model with versionToDeploy = Some(v)  }
        nextModel, Cmd.none    
    | ChooseNamespace n->
        printf "namespace chose %A" n
        let nextModel = { model with namespaceToDeployTo = n  }
        nextModel, Cmd.none    
    | ProductDeployed (Ok newId)->
        let nextModel = 
            { model with 
                productToDeploy = None; 
                versionToDeploy = None  
                versionsAvailableForSelectedProduct = None;
                deploymentCreated = Some(newId);
            }
        nextModel, Cmd.none    
    | CancelDeployProduct ->
        model, Cmd.none
    | _ -> model, Cmd.none
