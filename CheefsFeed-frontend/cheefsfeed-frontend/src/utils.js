export const getHeaders = (token, shouldHaveContentType = true) => {
    const headers = {
      Authorization: `Bearer ${token}`,
    };
  
    if (shouldHaveContentType) {
      headers["Content-Type"] = "multipart/form-data";
    }
  
    return headers;
  };