namespace Livingstone.Shared

open System
open System.Reflection.Emit
open System.ComponentModel


[<AutoOpen>]
module Application =

    [<CLIMutable>]
    type Application =
        {
            name: string
            port:int
        }

    let Initial = { name=""; port=10000; }

    let create  n p = { Initial with name=n; port=p;  }

    /// wrap an url to a blobStorage
    type FileStorage = |FileStorage of string
    type ApplicationName = |ApplicationName of string 

//     type Application =
//     | Microservice of Application
//     | SPA of Application
//     | BFF of Application

//     type SF5ComponentDto = {
//         Tag : string // one of "Microservice","SPA", "BFF"
//         // no data for A case
//         name : string  // data for B case
//         port : int
//         dependencies : NameDto        // data for D case 
//     }

//     let microService n p = Microservice({ Initial with name=n; port=p;  })
//     let spa n p = SPA({ Initial with name=n; port=p;  })
//     let bff n p = BFF({ Initial with name=n; port=p;  })

 

//     let isComponent x y =
//         match x,y with
//         | Microservice(a), Microservice(b) -> a.name = b.name
//         | SPA(a), SPA(b) -> a.name = b.name
//         | BFF(a), BFF(b) -> a.name = b.name
//         | _ -> false

    
    let isApplicationOption x y =
        match x with
        | Some (a) -> a.name = y.name
        | None -> false

    let getNameFromOptionOrDefault x d=
        match x with
        | Some (a) -> a.name
        | _ -> d


[<AutoOpen>]
module Version =

    [<CLIMutable>]
    type Version = 
        {
            SemVer : string            
        }

    let value v = v.SemVer

    let create v = { SemVer=v }
        

[<AutoOpen>]
module Component=


    [<CLIMutable>]
    type VersionnedComponent =
        {
            application: Application
            version: Version
        }

    let create c v = { application = c; version = v }


[<AutoOpen>]
module ComponentReadModel=

    [<CLIMutable>]
    type ComponentReadModel =
        {
            application: Application
            versions : Version[]
        }
    
    let Initial(app:Application):ComponentReadModel = {  application = app;versions = [||] }

    let create c v= { application = c;versions = v }
        

type Namespace = | Namespace of string

type TargetEnv =
    | DekraSubscription of Namespace
    | PocSubscription of Namespace
    | DevEnv of Namespace

[<AutoOpen>]
module ApplicationDeployment =

    [<CLIMutable>]
    type DeployApplicationCmd =
        {
            versionnedComponent: VersionnedComponent 
            targetEnv : TargetEnv
        }

[<AutoOpen>]
module Product =

    [<CLIMutable>]
    type Product =
        {
            id: Guid
            name: string
        }

    [<CLIMutable>]
    type VersionnedProduct =
        {
            product: Product
            version: Version
        }    

    [<CLIMutable>]
    type ProductVersionReadModel =     
        {
            versionnedProduct: VersionnedProduct
            Components : VersionnedComponent[]
        }    

    [<CLIMutable>]
    type ProductReadModel =     
        {
            product: Product
            versions: ProductVersionReadModel []
        }

    let create n v=
        {
            product=
                {
                    id=Guid.NewGuid()
                    name= n
                }
            versions = v
        }

[<AutoOpen>]
module ProductDeployment =

    [<CLIMutable>]
    type DeployProductCmd =
        {
            versionnedProduct: VersionnedProduct 
            targetEnv : TargetEnv
        }
    
    // let addComponent c p=
    //     { p with Components = Array.append p.Components [|c|] }

    // let addVersions Components p=
    //     { p with Components = Array.append p.Components Components }
    
    // let Initial():Product = {  id=Guid.NewGuid(); name= "";version = Version.Initial; Components = [||] }



    
