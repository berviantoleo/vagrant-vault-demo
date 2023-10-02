USERNAME=berviantoleo
APPNAME=dotnet-vault-example
cd app
docker build --tag "$USERNAME/$APPNAME" .
docker push "$USERNAME/$APPNAME"