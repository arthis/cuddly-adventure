module Livingstone.Deployment.Commands.Routes

open System
open System.Threading.Tasks

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open FSharp.Control.Tasks.V2

open Giraffe
open Saturn
open Livingstone.Shared


open Livingstone.Deployment.Readmodel.Repos


let deployApplication (ctx:Http.HttpContext) =
    task {
        let! cmd = ctx.BindJsonAsync<DeployApplicationCmd>()
        printf "Server deploy Application %A" cmd
        return Some <| Guid.NewGuid()
    }
    

let deployProduct (ctx:Http.HttpContext) =
    task {
        let! cmd = ctx.BindJsonAsync<DeployProductCmd>()
        printf "Server deploy Product %A" cmd
        return Some <|  Guid.NewGuid()    
    }

// let DeclareVersionnizedComponent cmd=
//     printf "Server Declare Versionnized Component %A" cmd
//     Guid.NewGuid()    


let handleCmd (f:Http.HttpContext->Task<Guid option>) next ctx  =
    task { 
        let! result = f ctx
        match result with
        | Some(a)-> 
            printf "success fetch"
            return! Successful.OK a next ctx
        | None -> 
            printf "failure fetch"
            return! RequestErrors.notFound (text "oops") next ctx
    }

let webApp  = router {
    
        post "/api/deployment/deployApplication" (handleCmd deployApplication)
        post "/api/deployment/deployProduct" (handleCmd deployProduct)
                

        // post "/api/administration/onComponentBuilt" (fun next ctx ->
        //     task {
        //         let! cmd = ctx.BindJsonAsync<OnComponentBuilt>()
        //         let deploymentId = onComponentBuilt cmd
        //         return! Successful.OK deploymentId next ctx
        //     }
        // )
    }



    
    
        