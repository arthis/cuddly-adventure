module Livingstone.Deployment.Application.Types

open System

open Livingstone.Shared



type Model = { 
    namespaceToDeployTo : string 
    Components: ComponentReadModel[] option
    ApplicationToDeploy: Application option 
    versionsAvailableForSelectedComponent: Version[] option 
    versionToDeploy: Version option 
    deploymentCreated : Guid option
}


type DeployApplicationMsg =
| ChooseComponent of ComponentReadModel
| ChooseVersion of Version
| ChooseNamespace of string
| DeployApplication
| ApplicationDeployed of Result<Guid, exn>
| ApplicationDeploymentFailed of Result<Guid, exn>
| CancelDeployAppication
| InitialComponentsLoaded of Result<ComponentReadModel[], exn>