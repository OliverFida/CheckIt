const path = require('path');

module.exports = {
    output: {
        path: path.join(__dirname, "build"),
        filename: 'index.bundle.js'
    },
    devServer: {
        port: 3000,
        historyApiFallback: true
    },
    module: {
        rules: [
            {
                test: /\.js$/,
                exclude: /node_modules/,
                use: {
                    loader: 'babel-loader'
                }
            },
            {
                test: /\.css$/,
                use: [
                    'style-loader',
                    'css-loader'
                ]
            },
            {
                test: /\.less$/,
                use: [
                    "style-loader",
                    "css-loader",
                    {
                        loader: "less-loader",
                        options: {
                            lessOptions: {
                                modifyVars: {
                                    'primary-color': '#440099'
                                },
                                javascriptEnabled: true
                            }
                        }
                    }
                ]
            },
            {
                test: [/\.png$/, /\.jpg$/],
                use: {
                    loader: 'file-loader'
                }
            }
        ]
    }
};