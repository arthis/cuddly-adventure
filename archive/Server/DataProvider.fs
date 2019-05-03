namespace Livingstone.DataProvider

[<AutoOpen>]
module DataProvider =

    open System

    open Livingstone.Shared
     
        
    let applicationSample = 
        [|
            [|
                Version.create "0.1.0"
                Version.create "0.2.0"
                Version.create "0.3.0"
            |]
            |> ComponentReadModel.create (Application.create "iv-da" 1001)
            [|
                Version.create "0.1.0"
                Version.create "0.2.0"
            |]
            |> ComponentReadModel.create (Application.create "fb-iv-bff"  1002)
            [|
                Version.create "0.1.1"
            |]
            |> ComponentReadModel.create (Application.create "fb-iv-spa"  1003)
            [|
                Version.create "0.1.0"
            |]
            |> ComponentReadModel.create (Application.create "svc-pdfexport"  1004)
        |]

    let productAll =
        {
            id= Guid.Parse("6152cfc0-2c01-422b-9fc1-499fabddcae3")
            name= "productAll"
        }        
    let versionProduct = 
        { 
            product= productAll
            version = (Version.create "0.1.1")
        }

    let readModel =
        {
            versionnedProduct = versionProduct
            Components = 
            [|
                Component.create (Application.create "iv-da" 1001)  (Version.create "0.1.0")
                Component.create (Application.create "fb-iv-bff"  1002) (Version.create "0.1.1")
                Component.create (Application.create "fb-iv-spa"  1003) (Version.create "0.1.0")
                Component.create (Application.create "svc-pdfexport"  1004) (Version.create "0.1.0")
            |]
        }        

    let productSample = [|
        readModel
    |]    

    type GlobalState =
        {
            applications: ComponentReadModel[]
            products: ProductVersionReadModel[]
        }

    let create a p =
        {
            applications= a
            products=p
        }