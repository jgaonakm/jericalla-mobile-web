# Stage 1: Build
FROM node:18-alpine AS builder
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build

# Stage 2: Run
FROM node:18-alpine
WORKDIR /app
COPY --from=builder /app ./
EXPOSE 3000
ENV NODE_ENV=production
CMD ["npm", "start"]
