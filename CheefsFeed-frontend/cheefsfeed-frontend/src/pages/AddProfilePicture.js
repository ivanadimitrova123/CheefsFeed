import React, { useContext, useState } from "react";
import Navbar from "../components/navbar";
import { Store } from "../Store";
import { useNavigate } from "react-router-dom";
import axios from "../axios/axios";

function ImageUpload() {
  const navigate = useNavigate();
  const { state, dispatch: ctxDispatch } = useContext(Store);
  const { userInfo } = state;
  const [selectedFile, setSelectedFile] = useState(null);

  const handleFileChange = (e) => {
    setSelectedFile(e.target.files[0]);
  };
  
  const handleUpload = async () => {
    if (!selectedFile) {
      alert("Please select a file to upload.");
      return;
    }
  
    const formData = new FormData();
    formData.append("file", selectedFile);
  
    const headers = {
      "Authorization": `Bearer ${userInfo.token}`
    };
  
    try {
      const response = await axios.post("image", formData, { headers });
  
      if (response.status === 200) {
        const data = response.data;
        const updatedUser = { ...userInfo.user, picture: data.imageUrl };
        ctxDispatch({ type: "UPDATE_PICTURE", payload: updatedUser });
        navigate(`/userProfile/${userInfo.user.id}`);
      } else {
        alert(`Upload failed: ${response.statusText}`);
      }
    } catch (error) {
      console.error("Upload error:", error.response ? error.response.data : error.message);
      alert("An error occurred while uploading the image.");
    }
  };
  

  const handleConfirm = () => {
    handleUpload();
  };

  const handleCancel = () => {
    setSelectedFile(null);
  };

  return (
    <div className="container-fluid customBackground addProfilePicture">
      <Navbar />
      <div className="imageChangeContainer">
      {!selectedFile &&(
        <div className="changeImageBox">
        <h2>Choose New Profile Picture</h2>
        <div className="form-group mt-4">
          <input
            type="file"
            accept="image/*"
            className="form-control-file"
            onChange={handleFileChange}
          />
        </div>
      </div>
      )}
        {selectedFile && (
          <div className="changedImage">
            <img
              src={URL.createObjectURL(selectedFile)}
              alt="Selected"
              style={{ maxWidth: "100%", maxHeight: "300px" }}
            />
            <div className="changeImgButtons">
              <p>Are you sure you want to upload this image?</p>
              <button onClick={handleConfirm} className="btn btn-primary">
                Save and Proceed
              </button>
              <button onClick={handleCancel} className="btn btn-secondary">
                Pick Another
              </button>
            </div>
          <div>
          </div>
          </div>
        )}
      </div>
    </div>
  );
}

export default ImageUpload;