module Livingstone.Deployment.Product.Types


open System
open Livingstone.Shared




type Model = { 
    namespaceToDeployTo : string 
    products: ProductVersionReadModel[] option
    productToDeploy: Product option 
    versionsAvailableForSelectedProduct: Version[] option 
    versionToDeploy: Version option 
    deploymentCreated : Guid option
}


type DeployProductMsg =
| DeployProduct
| ChooseProduct of ProductVersionReadModel
| ChooseVersion of Version
| ChooseNamespace of string
| CancelDeployProduct
| InitialProductLoaded of Result<ProductVersionReadModel [], exn>
| ProductDeployed of Result<Guid, exn>
| ProductDeploymentFailed of Result<Guid, exn>