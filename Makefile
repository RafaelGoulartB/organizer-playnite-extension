.PHONY: build clean restore publish

build:
	dotnet build src/PnExtension.csproj

clean:
	dotnet clean src/PnExtension.csproj

restore:
	dotnet restore src/PnExtension.csproj

publish:
	dotnet publish src/PnExtension.csproj -c Release -o ./Release
