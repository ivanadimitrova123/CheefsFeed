import React, { useContext } from "react";
import flag from "../assets/images/flag-black-shape.svg";
import { Store } from "../Store";
import axios from "../axios/axios"; 
import { toast } from "react-toastify";
import { getHeaders } from "../utils";
const Comment = ({ comment, deleteHandler }) => {
  const { state } = useContext(Store);
  const { userInfo } = state;
  //const [reportIsLoading, setReportIsLoading] = useState(false);

  const reportHandler = async () => {
    const headers = getHeaders(userInfo.token);

    //setReportIsLoading(true);

    const formData = new FormData();
    formData.append("userId", userInfo.user.id);
    formData.append("commentId", comment.commentId);
    axios
      .post(`/reportcomment`, formData, { headers })
      .then((response) => {
        //setReportIsLoading(false);
        toast.success(response.data);
      })
      .catch((error) => {
       // setReportIsLoading(false);
        console.error("Error reporting comment:", error);
      });
  };

  return (
    <div
      key={comment.id}
      style={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      }}
    >
      <div>
        <img
          src={comment.userImage}
          style={{ width: "40px", height: "40px", borderRadius: "50%" }}
          alt="profile"
        />
        <b style={{ marginLeft: "5px" }}>{comment.username}</b>
        <p>{comment.content}</p>
      </div>
      {userInfo.user.username !== comment.username ? (
        <img
          onClick={reportHandler}
          src={flag}
          alt="report"
          style={{
            width: "20px",
            height: "20px",
            marginRight: "20px",
            cursor: "pointer",
          }}
        />
      ) : (
        <button
          className="btn" style={{  background: "#ff1540", 
          color: "white",
          borderRadius: "15px",
          padding: "5px 20px"
          }}
          onClick={() => deleteHandler(comment.commentId)}
        >
          Delete
        </button>
      )}
    </div>
  );
};

export default Comment;
