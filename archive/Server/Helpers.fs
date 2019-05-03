

[<AutoOpen>]
module Livingstone.Helpers


open System
open System.Threading.Tasks

open Microsoft.AspNetCore
open Microsoft.AspNetCore.Http
open Microsoft.AspNetCore.Builder
open FSharp.Control.Tasks.V2
open System.Threading.Tasks


open Giraffe

open Saturn
open Livingstone.Shared

type HandleCmd<'a, 'error> =
     AsyncResult<'a,'error> -> HttpContext -> HttpFunc    -> Task<HttpContext option>

let handleCmd<'a, 'error>  : HandleCmd< 'a, 'error> =
    fun result ctx next    -> 
        task { 
            let! r = Async.StartAsTask result 
            match r with
            | Ok events -> 
                return! Successful.OK events next ctx 
            | Error err ->
                return! RequestErrors.notFound (text "oops") next ctx
 
        }

let handleRead<'a> (f:Http.HttpContext->Task<'a option>) next ctx  =
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
