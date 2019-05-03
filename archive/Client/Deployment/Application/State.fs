module Livingstone.Deployment.Application.State


open Elmish
open Livingstone.Deployment.Application.Rest
open Livingstone.Deployment.Application.Types
open Livingstone.Shared

// defines the initial state and initial command (= side-effect) of the application
let init () : Model * Cmd<DeployApplicationMsg> =
    let initialModel = { 
        Components = None
        namespaceToDeployTo = "ci"
        versionsAvailableForSelectedComponent = None
        ApplicationToDeploy = None
        versionToDeploy = None
        deploymentCreated = None
    }
    let loadCountCmd =
        Cmd.ofPromise
            initialComponents
            []
            (Ok >> InitialComponentsLoaded)
            (Error >> InitialComponentsLoaded)
    initialModel, loadCountCmd

// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
let update (msg : DeployApplicationMsg) (model : Model) : Model * Cmd<DeployApplicationMsg> =
    match msg with
    | InitialComponentsLoaded (Ok components)->
        printf "InitialComponentsLoaded"
        let nextModel = { 
            namespaceToDeployTo = "ci"
            Components = Some components ;
            versionsAvailableForSelectedComponent = None
            ApplicationToDeploy = None;
            versionToDeploy = None;
            deploymentCreated = None
        }
        nextModel, Cmd.none
    | InitialComponentsLoaded (Error ex)->        
        printf "InitialComponentsLoaded error %A" ex
        model, Cmd.none
    | DeployApplication ->
        let deployApplicationCmd =
            match   model.ApplicationToDeploy, model.versionToDeploy  with
            | Some (c), Some(v) ->
                     let cmd = { versionnedComponent={ application= c; version=v};  targetEnv = DekraSubscription <| Namespace model.namespaceToDeployTo }                    
                     Cmd.ofPromise
                        (deployApplication cmd)
                        []
                        (Ok >> ApplicationDeployed)
                        (Error >> ApplicationDeploymentFailed)
 
            | _ -> failwith "cannot deploy with no Component chosen or version."
        model, deployApplicationCmd
    | ChooseComponent c->
        printf "containzer chose %A" c
        let nextModel = 
            { model with 
                ApplicationToDeploy = Some(c.application); 
                versionToDeploy = None  
                versionsAvailableForSelectedComponent = Some(c.versions);
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
    | ApplicationDeployed (Ok newId)->
        let nextModel = 
            { model with 
                ApplicationToDeploy = None; 
                versionToDeploy = None  
                versionsAvailableForSelectedComponent = None;
                deploymentCreated = Some(newId);
            }
        nextModel, Cmd.none
    | CancelDeployAppication ->
        model, Cmd.none
    | _ -> model, Cmd.none
 