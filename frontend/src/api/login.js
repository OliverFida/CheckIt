const debugUsername = "dageorg";
const debugPassword = "linux";

const debugResponse200 = {
    "value": "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJJZCI6IjA1MTZiZjYwLTljNjctNGUxMi05ZWQ3LThlMTk0NWI3NGRkNSIsInN1YiI6ImFkbWluIiwiZW1haWwiOiJhZG1pbiIsImp0aSI6IjdmYmRhYmU0LTkzMDktNDVmZC1hNTMwLWZiNDhhMjIyYmRhYiIsInJvbGUiOlsiQWRtaW4iLCJVc2VyIl0sIm5iZiI6MTY2NDUyMTc0NSwiZXhwIjoxNjY0NTIyMDQ1LCJpYXQiOjE2NjQ1MjE3NDUsImlzcyI6Imh0dHBzOi8vYWRzZi5jb20vIiwiYXVkIjoiaHR0cHM6Ly9hZHNmLmNvbS8ifQ.VEK9ArysnJN6CR6cW30BE68TfmdKRAeqhq2aOvFAbh2-XZuZ72XULvhnh-y3WGKYL-f8sStfZlfGAfl7NUNAPQ",
    "statusCode": 200,
    "contentType": null
};

const debugResponse400 = {
    "statusCode": 400
};

function login(username, password){
    if(username === debugUsername && password === debugPassword) return debugResponse200;
    return debugResponse400;
}

module.exports = {
    login
};