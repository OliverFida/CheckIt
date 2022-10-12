import base from './base';

async function getUsers(){
    return await base.apiRequest('admin/users', "GET", null);
}

async function addUser(username, firstName, lastName, role){
    return await base.apiRequest('admin/user', "PUT", {
        "username": username,
        "firstName": firstName,
        "lastname": lastName,
        "role": role,
        "password": "awl"
    });
}

async function editUser(username, firstName, lastName, role){
    return await base.apiRequest(`admin/user/${username}`, "PATCH", {
        "firstName": firstName,
        "lastName": lastName,
        "role": role
    });
}

async function resetPassword(username){
    return await base.apiRequest(`admin/user/${username}/password`, "PATCH", {
        "password": "awl"
    });
}

async function setUserActive(username, state){
    if(state) return await base.apiRequest(`admin/user/${username}/activate`, "POST", null);
    return await base.apiRequest(`admin/user/${username}/deactivate`, "POST", null);
}

async function deleteUser(username){
    return await base.apiRequest(`admin/user/${username}`, "DELETE", null);
}

async function getRoles(){
    return await base.apiRequest('admin/roles', "GET", null);
}

var exports = {
    getUsers,
    addUser,
    editUser,
    resetPassword,
    setUserActive,
    deleteUser,
    getRoles
};
export default exports;