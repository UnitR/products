const {createProxyMiddleware} = require('http-proxy-middleware');

module.exports = function (app) {
    app.use(
        '/api',
        createProxyMiddleware({
            target: process.env.services__api__http__0,
            changeOrigin: true,
            secure: false,
            pathRewrite: {
                '^/api': '',
            }
        }),
    );
};
