# kill all dotnet processes under acc. name "stevie", use SIGTERM (15) kill signal
echo "Attempting to terminate all dotnet processes..."
pkill -15 dotnet -u stevie

echo "Pulling from git..."
git pull

echo "Updating submodules..."
git submodule update --init --recursive

echo "Building TomatBot..."
dotnet build TomatBot.sln --configuration Release

echo "Executing TomatBot..."
dotnet Tomat.TomatBot/bin/Release/net5.0/Tomat.TomatBot.dll