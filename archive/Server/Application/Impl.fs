
namespace Livingstone.Application

open System
open FSharp.Control.Tasks.V2
open Livingstone.Shared.Application
open InternalTypes

module ReadModel =
    let toto = "tata"

module internal Impl =

    open Livingstone.Shared
    open Livingstone.Application.PublicTypes
             
    let validateBuildInfos: ValidateBuildInfos =
        fun checkApplicationExists unvalidatedBuildInfos ->
            asyncResult {
                let validatedBuildInfos: ValidatedBuildInfos =  {
                    version = unvalidatedBuildInfos.version
                    helmValues = unvalidatedBuildInfos.helmValues
                    data = unvalidatedBuildInfos.data
                    applicationName = unvalidatedBuildInfos.applicationName
                }
                return validatedBuildInfos
            }

    let addBuildInfos : AddBuildInfos =
        fun getApplication validatedBuildInfos-> 
            asyncResult {
                let! application = 
                    getApplication validatedBuildInfos.applicationName

                let newBuild = {
                    version= validatedBuildInfos.version
                    helmValues= validatedBuildInfos.helmValues
                    data= validatedBuildInfos.data
                }
                return KnownApplication({ application with builds= newBuild:: application.builds})
            }

    let createEvents : CreateEvents=
        fun application -> [BuildAddedToApplication({name="xx-yy"; newBuild="1.2.3"})]

    let addBuildInformation  
        checkApplicationExists
        getApplication
        saveApplication
        publishEvents
        :AddBuild =
        (fun  unvalidatedBuildInfos ->
            asyncResult {

                let! validatedBuildInfos =
                    validateBuildInfos checkApplicationExists unvalidatedBuildInfos
                    |> AsyncResult.mapError ApplicationError.Validation

                let! application = 
                    validatedBuildInfos
                    |> addBuildInfos getApplication 
                    |> AsyncResult.mapError ApplicationError.ApplicationNotFound

                do! saveApplication application
                    |> AsyncResult.mapError ApplicationError.SaveError                                

                let events = 
                    createEvents application
                    
                
                do! publishEvents events 
                    |> AsyncResult.mapError ApplicationError.PublishError                                

                
                return application,events
                    
                
            }

        )


