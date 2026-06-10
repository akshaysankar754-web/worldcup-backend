pipeline {
    agent any
 
  environment {
    CONTAINER_NAME = 'worldcup-backend-jenkins'
    IMAGE_NAME = 'worldcup-backend'

    NETWORK_NAME = 'app-net'

    DB_CONTAINER = 'app-mysql'

    PORT_MAPPING = '5085:8080'

    DB_CONNECTION = "Server=app-mysql;Port=3306;Database=footballpollingdb;User=root;Password=root;"

    JWT_SECRET = 'super_secret_key_world_cup_polling_system_2026_very_long_for_hmac_sha256'
    JWT_ISSUER = 'WorldCupApp'
    JWT_AUDIENCE = 'WorldCupUsers'
}
 
    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }
 
        stage('Build Docker Image') {
            steps {
                bat "docker build --no-cache -t ${IMAGE_NAME}:latest -t ${IMAGE_NAME}:${BUILD_NUMBER} ."
            }
        }
 
        stage('Deploy Container') {
            steps {
                script {
                    // Ensure Docker network exists (ignoring error if it already exists)
                    bat "docker network create ${NETWORK_NAME} 2>nul || ver >nul"
                   
                    // Stop and remove existing container if running
                    bat "docker stop ${CONTAINER_NAME} 2>nul || ver >nul"
                    bat "docker rm ${CONTAINER_NAME} 2>nul || ver >nul"
                   
                    // Launch new container using Windows Batch line continuation
                    bat """
                        docker run -d ^
                            --name ${CONTAINER_NAME} ^
                            --network ${NETWORK_NAME} ^
                            -p ${PORT_MAPPING} ^
                            -e ConnectionStrings__DefaultConnection="${DB_CONNECTION}" ^
                            -e Jwt__Key="${JWT_SECRET}" ^
                            -e Jwt__Issuer="${JWT_ISSUER}" ^
                            -e Jwt__Audience="${JWT_AUDIENCE}" ^
                            -e RUN_MIGRATIONS=true ^
                            -e ENABLE_SWAGGER=true ^
                            ${IMAGE_NAME}:latest
                    """
                }
            }
        }
    }
 
    post {
        success {
            echo "Backend pipeline completed successfully!"
        }
        failure {
            echo "Backend pipeline failed. Please check the logs."
        }
    }
}
 