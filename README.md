# Sixpence WeChat Server

> 微信公众号&小程序的管理平台后端项目

## Deploy

### Docker

```bash
docker buildx build --platform=linux/amd64 -t carldu/wechat-server:latest -f "./Sixpence.WeChat/Dockerfile" --label "com.microsoft.created-by=visual-studio" --label "com.microsoft.visual-studio.project-name=Sixpence.WeChat" "." # build docker iamge

docker push carldu/wechat-server:latest # publish iamge

docker run -d -p 5050:5000 --name wechat-server carldu/wechat-server:latest # run
```
