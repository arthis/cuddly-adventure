module Livingstone.App

open System
open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Giraffe.ModelBinding
open FSharp.Control.Tasks.V2
open Giraffe

open Saturn.CSRF.View
open Saturn

open Livingstone.Shared
open Livingstone.Deployment.Readmodel
let publicPath = Path.GetFullPath "../Client/public"
let port = 8085us

open Livingstone.DataProvider


let deployApplication cmd =
    printf "Server deploy Application %A" cmd
    Guid.NewGuid()

let deployProduct cmd =
    printf "Server deploy Product %A" cmd
    Guid.NewGuid()    

let DeclareVersionnizedComponent cmd=
    printf "Server Declare Versionnized Component %A" cmd
    Guid.NewGuid()    


let handleRead<'a> (f:Http.HttpContext->Task<'a option>) next ctx  =
    task { 
        let! result = f ctx
        match result with
        | Some(a)-> return! Successful.OK a next ctx
        | None -> return! RequestErrors.notFound (text "oops") next ctx
    }



let webApp dataprovider = choose [

    Livingstone.Deployment.Readmodel.Routes.webApp dataprovider
    Livingstone.Deployment.Commands.Routes.webApp    
]


let configureSerialization (services:IServiceCollection) =
    services.AddSingleton<Giraffe.Serialization.Json.IJsonSerializer>(Thoth.Json.Giraffe.ThothSerializer())

let dataProvider = DataProvider.create DataProvider.applicationSample DataProvider.productSample    

let app = application {
    url ("http://0.0.0.0:" + port.ToString() + "/")
    use_router (webApp dataProvider)
    memory_cache
    use_static publicPath
    service_config configureSerialization
    use_gzip
}

run app
