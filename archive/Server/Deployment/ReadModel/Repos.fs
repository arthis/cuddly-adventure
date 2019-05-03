module Livingstone.Deployment.Readmodel.Repos

open System
open System.Threading.Tasks

open FSharp.Control.Tasks.V2
open Livingstone.Shared
open Livingstone.DataProvider


let getComponents dataprovider ctx =
    Task.FromResult(Some(dataprovider.applications))



let getProducts dataprovider ctx =
    Task.FromResult(Some(dataprovider.products))