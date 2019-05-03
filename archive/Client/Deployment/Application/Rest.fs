module Livingstone.Deployment.Application.Rest

open System
open Fable.PowerPack.Fetch

open Thoth.Json

open Livingstone.Deployment.Application.Types
open Livingstone.Shared
open Livingstone.RestHelpers

let initialComponents = fetchAs<ComponentReadModel[]> "/api/deployment/getComponents" (Decode.Auto.generateDecoder())

let deployApplication = post<DeployApplicationCmd,Guid> "/api/deployment/deployApplication" (Decode.Auto.generateDecoder())