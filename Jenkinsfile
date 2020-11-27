pipeline {
  agent any

  environment {
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

    stage('deploy'){
      when {
        branch 'master'
      }
      steps{
        VersionCode="2020.11.25"
        ProjectName="RPN.milkcocoa.info"
        sh """
cd ${ProjectName}
nuget spec
dotnet pack --configuration Release -p:PackageVersion=${VersionCode}
"""
//dotnet nuget push bin/Release/${ProjectName}.${VersionCode}.nupkg --api-key ${CREDENTIALS} --source https://api.nuget.org/v3/index.json
//"""
      }
    }

		stage('upload artifact'){
			steps{
				archiveArtifacts allowEmptyArchive: true, artifacts: '**/*.nupkg', fingerprint: true, onlyIfSuccessful: true
			}
		}
  }
}
}
