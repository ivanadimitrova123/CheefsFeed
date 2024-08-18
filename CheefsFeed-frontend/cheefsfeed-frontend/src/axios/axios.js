import a from "axios";
const url = "https://localhost:44365/api/";

const axios = a.create({
  baseURL: url,
});

export default axios;