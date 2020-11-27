pipeline {
  agent any

environment{
		VersionCode = (new Date()).format("yyyy.MM.dd")
		ProjectName = "RPN.milkcocoa.info"
    CREDENTIALS = credentials("NuGet-API-Key")
  }

  stages {
    stage('build') {
      steps {
        sh 'dotnet build --configuration Release'
      }
    }

    stage('unit test'){
      steps{
        sh 'dotnet test --configuration Release'
      }
    }
		stege('genearte package'){
			steps{
				dir(ProjectName){
        	sh 'nuget spec'
					sh "dotnet pack --configuration Release -p:PackageVersion=${VersionCode}"
				}
			}
		}

    stage('deploy'){
      when {
        branch 'master'
      }
      steps{
				dir(ProjectName){
					sh "dotnet nuget push bin/Release/${ProjectName}.${VersionCode}.nupkg --api-key ${CREDENTIALS} --source https://api.nuget.org/v3/index.json"
	      }
  	  }
		}

		stage('upload artifact'){
			steps{
				archiveArtifacts allowEmptyArchive: true, artifacts: '**/*.nupkg', fingerprint: true, onlyIfSuccessful: true
			}
		}
		stage('clean up'){
			steps{
				cleanWs()
			}
		}
  }
}
