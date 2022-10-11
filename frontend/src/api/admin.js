const apiRequest = require('./base').apiRequest;

async function getUsers(){
    return await apiRequest('admin/users', "GET", null);
}

async function addUser(username, firstName, lastName){
    return await apiRequest('admin/user', "PUT", {
        "username": username,
        "firstName": firstName,
        "lastname": lastName,
        "role": "User",
        "password": "awl"
    });
}

async function editUser(username, firstName, lastName, role){
    return await apiRequest(`admin/user/${username}`, "PATCH", {
        "firstName": firstName,
        "lastName": lastName,
        "role": role
    });
}

async function resetPassword(username){
    return await apiRequest(`admin/user/${username}/password`, "PATCH", "awl");
}

async function setUserActive(username, state){
    if(state) return await apiRequest(`admin/user/${username}/activate`, "POST", null);
    return await apiRequest(`admin/user/${username}/deactivate`, "POST", null);
}

async function deleteUser(username){
    return await apiRequest(`admin/user/${username}`, "DELETE", null);
}

module.exports = {
    getUsers,
    addUser,
    editUser,
    resetPassword,
    setUserActive,
    deleteUser
};