const possiblePasswordChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!?.,-_#";

function generatePassword(){
    const pwLength = 8;
    var temp = "";

    var rnd;
    for(var i = 1; i <= pwLength; i++){
        rnd = Math.floor(Math.random() * possiblePasswordChars.length);
        temp += possiblePasswordChars[rnd];
    }

    return temp;
}

const exports = {
    generatePassword
};
export default exports;