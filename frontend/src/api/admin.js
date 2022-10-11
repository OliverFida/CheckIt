const apiRequest = require('./base').apiRequest;

async function getUsers(){
    return await apiRequest('admin/users', "GET", null);
}
module.exports = {
    getUsers
};