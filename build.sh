docker build -f Dockerfile -t watcher-aspnet .
docker build -f Dockerfile.IISServiceMonitor -t service-monitor .
docker build -f Dockerfile.simple -t monitor .
