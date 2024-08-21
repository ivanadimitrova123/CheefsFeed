import React, { useContext, useState } from "react";
import Navbar from "../components/navbar";
import { Store } from "../Store";
import { useNavigate } from "react-router-dom";

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

    const headers = new Headers();
    headers.append("Authorization", `Bearer ${userInfo.token}`);

    try {
      const response = await fetch(
        "https://localhost:44365/api/image",
        {
          method: "POST",
          body: formData,
          headers,
        }
      );

      if (response.ok) {
        const data = await response.json();
        var u = userInfo.user;
        u.picture = data.imageUrl;
        ctxDispatch({ type: "UPDATE_PICTURE", payload: u });
        navigate(`/userProfile/${userInfo.user.id}`);
      } else {
        const data = await response.json();
        alert(`Upload failed: ${data}`);
        console.log(data);
      }
    } catch (error) {
      alert("An error occurred while uploading the image.");
      console.error(error);
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