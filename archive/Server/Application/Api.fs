namespace Livingstone.Application

open Microsoft.AspNetCore
open System.Threading.Tasks
open Microsoft.AspNetCore.Builder
open FSharp.Control.Tasks.V2

open Giraffe
open Saturn


open Microsoft.AspNetCore.Http

open Livingstone.Helpers
open PublicTypes
open InternalTypes


module Api=

    let checkApplicationExists : CheckApplicationExists =
        fun name -> true

    let getApplication: GetApplication =
        fun appName -> asyncResult {
            return {
                name = "xx-yy"
                port= 1200
                builds = []
            }
        }

    let saveApplication : SaveApplication = 
        fun app -> asyncResult {
            return ()
        }
        
    let publishEvents : PublishEvents     =
        fun events -> asyncResult {
            return ()
        }

    let addBuildInformation dto =
        asyncResult {
            let! (application,events) =
                dto 
                |> toUnvalidatedBuildInfos 
                |> Impl.addBuildInformation
                    checkApplicationExists //dependency
                    getApplication //dependency
                    saveApplication
                    publishEvents

            

            return events
        }        


    let handleAddBuildInfos =
        fun (next:HttpFunc) (ctx:HttpContext)  ->
            task {
                let! dto = ctx.BindJsonAsync<BuildInfosDto>()

                let result = addBuildInformation dto
                return! handleCmd result ctx next
            }

    let webApp  = router {
       post "/api/appplication/onBuildInfoAdded"  handleAddBuildInfos 
    }