const path = require('path')
const webpack = require('webpack')
/**
* [hash]
* [name]
* [chunkhash]
* [id]
* [query]
* [contenthash]
*/
module.exports = {
    mode:'development',
    entry: {
        'loginRegister': './SJLogin/LoginRegister.ts',
        'dashboard': './Dashboard.ts',
        'adddevice': './adddevice.ts',
        'request': './Request.ts',
        'getApiForAdmin': './getApiForAdmin.ts',
        'faultIndex': './faultyDevice/faultIndex.ts',
        'getNotification': './getNotification.ts',
        'index': './request-history/index.ts', // No its not the main file. Just a file required by a page
        'specificationcrud': './specificationcrud.ts',
        'submissionRequest': './submissionRequest.ts',
        'request-model': './request-model.ts',
        'userHistory': './userHistory.ts',
        'webpage': './webpage.ts',
        'main': './user-profile/main.ts',
        'userRequestMain': './Device-Request/UserRequestMain.ts',
        'devicedetail': './devicedetail.ts',
        'user_role': './device_role/user_role.ts',
        'forgotpassword':'./SJLogin/forgotpassword.ts'

    },
    resolve: {
        extensions: [".js", ".ts"]
    },
    output:{
        path: __dirname,
        filename:'js/[name].js',//webpage.js other js
        publicPath:'/assets/'
        // libraryTarget:'umd',
        // library:'myFirstLibrary'
    },
    module: {
        rules: [
            {
                test: /\.ts$/,
                loader: 'ts-loader'
            }
        ]
    },
    devServer:{
        port:1234,
        contentBase: __dirname,
        headers: {
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Headers': 'Content-Type, Authorization, x-id, Content-Length, X-Requested-With'
        }
    }
}

