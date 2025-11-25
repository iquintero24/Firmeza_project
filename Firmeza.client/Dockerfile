# ============================
# 1) BUILD DEL FRONTEND (VITE)
# ============================
FROM node:18 AS builder

WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .

RUN npm run build


# ============================
# 2) SERVIR CON NGINX
# ============================
FROM nginx:alpine

COPY --from=builder /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]
