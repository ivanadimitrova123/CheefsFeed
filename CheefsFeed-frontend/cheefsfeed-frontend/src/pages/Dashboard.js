import React, { useContext, useEffect, useState } from "react";
import Navbar from "../components/navbar";
import { Store } from "../Store";
import axios from "../axios/axios";
import { useNavigate } from "react-router-dom";
import { Spinner } from "react-bootstrap";
import { getHeaders } from "../utils";

const Dashboard = () => {
  const navigate = useNavigate();
  const { state } = useContext(Store);
  const { userInfo } = state;
  const [comments, setComments] = useState([]);
  const [commentsIsLoading, setCommentsIsLoading] = useState(false);
  const [recipes, setRecipes] = useState([]);
  const [recipesIsLoading, setRecipesIsLoading] = useState(false);
  const [commentRefesh, setCommentRefesh] = useState(false);
  const [commentDeleteLoading, setCommentDeleteLoading] = useState(false);
  const [recipeDeleteLoading, setRecipeDeleteLoading] = useState(false);
  const [recipeRefresh, setRecipeRefresh] = useState(false);
  const [commentAllowLoading, setCommentAllowLoading] = useState(false);
  const [recipeAllowLoading, setRecipeAllowLoading] = useState(false);

  const deleteHandler = async (id) => {
    const headers = getHeaders(userInfo.token, false);
    setCommentDeleteLoading(true);
    try {
      await axios.delete(`/reportcomment/delete/${id}`, { headers });
      setCommentDeleteLoading(false);
      setCommentRefesh(true);
    } catch (error) {
      setCommentDeleteLoading(false);
      console.error("Error deleting comment:", error);
    }
  };

  const deleteRecipe = async (id) => {
    const headers = getHeaders(userInfo.token, false);
    setRecipeDeleteLoading(true);
    try {
      await axios.delete(`/reportedrecipe/delete/${id}`, { headers });
      setRecipeDeleteLoading(false);
      setRecipeRefresh(true);
    } catch (error) {
      setRecipeDeleteLoading(false);
      console.error("Error deleting recipe:", error);
    }
  };

  const allowComment = async (id) => {
    const headers = getHeaders(userInfo.token, false);
    setCommentAllowLoading(true);
    try {
      await axios.delete(`/reportcomment/${id}`, {}, { headers });
      setCommentAllowLoading(false);
      setCommentRefesh(true);
    } catch (error) {
      setCommentAllowLoading(false);
      console.error("Error allowing comment:", error);
    }
  };

  const allowRecipe = async (id) => {
    const headers = getHeaders(userInfo.token, false);
    setRecipeAllowLoading(true);
    try {
      await axios.delete(`/reportedrecipe/${id}`, {}, { headers });
      setRecipeAllowLoading(false);
      setRecipeRefresh(true);
    } catch (error) {
      setRecipeAllowLoading(false);
      console.error("Error allowing recipe:", error);
    }
  };

  useEffect(() => {
    setCommentsIsLoading(true);
    const headers = getHeaders(userInfo.token, false);
    axios
      .get("/reportcomment", { headers })
      .then((response) => {
        setComments(response.data);
        setCommentsIsLoading(false);
      })
      .catch((error) => {
        console.error("Error fetching reported comments:", error);
        setCommentsIsLoading(false);
      });

    if (commentRefesh) setCommentRefesh(false);
  }, [commentRefesh, userInfo]);

  useEffect(() => {
    setRecipesIsLoading(true);
    const headers = getHeaders(userInfo.token, false);
    axios
      .get("/reportedrecipe", { headers })
      .then((response) => {
        setRecipes(response.data);
        setRecipesIsLoading(false);
      })
      .catch((error) => {
        console.error("Error fetching reported recipes:", error);
        setRecipesIsLoading(false);
      });

    if (recipeRefresh) setRecipeRefresh(false);
  }, [userInfo, recipeRefresh]);

  return (
    <div className="container-fluid customBg" style={{ minHeight: "100vh" }}>
      <Navbar />
      <div className="row mb-3">
        <div className="col" style={{ display: "flex", alignItems: "center" }}>
          <h2
            className="mainHeader"
            style={{ width: "100%", textAlign: "center" }}
          >
            Admin Dashboard
          </h2>
        </div>
      </div>
      <div
        className="mt-5"
        style={{ display: "flex", justifyContent: "space-evenly" }}
      >
        {/* Reported Comments Section */}
        <div style={{ width: "40%" }}>
          <h2>Reported Comments</h2>
          {commentsIsLoading && (
            <div className="text-center">
              <p>Loading...</p>
              <div className="spinner-border" role="status">
                <span className="visually-hidden">Loading...</span>
              </div>
            </div>
          )}
          {comments &&
            comments.map((c) => (
              <div
                className="mt-3"
                key={c.commentId}
                style={{
                  border: "1px solid black",
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "center",
                  padding: "10px",
                }}
              >
                <div>
                  <img
                    src={c.user.img}
                    alt="profile"
                    style={{ width: "30px", height: "30px" }}
                  />
                  <b className="ms-2">{c.user.username}</b>
                  <span className="ms-3">{c.comment.content}</span>
                </div>
                <div>
                  <button
                    className="me-2 btn btn-primary"
                    onClick={() => navigate(`/recipeDetails/${c.comment.recipeId}`)}
                  >
                    View
                  </button>
                  <button
                    className="me-2 btn btn-danger"
                    onClick={() => deleteHandler(c.commentId)}
                  >
                    {commentDeleteLoading ? (
                      <Spinner style={{ width: "1rem", height: "1rem" }} />
                    ) : (
                      "Delete"
                    )}
                  </button>
                  <button
                    className="me-2 btn btn-success"
                    onClick={() => allowComment(c.commentId)}
                  >
                    {commentAllowLoading ? (
                      <Spinner style={{ width: "1rem", height: "1rem" }} />
                    ) : (
                      "Allow"
                    )}
                  </button>
                </div>
              </div>
            ))}
        </div>
        {/* Reported Recipes Section */}
        <div style={{ width: "40%" }}>
          <h2>Reported Recipes</h2>
          {recipesIsLoading && (
            <div className="text-center">
              <p>Loading...</p>
              <div className="spinner-border" role="status">
                <span className="visually-hidden">Loading...</span>
              </div>
            </div>
          )}
          {recipes &&
            recipes.map((r) => (
              <div
                className="mt-3"
                key={r.recipeId}
                style={{
                  border: "1px solid black",
                  display: "flex",
                  justifyContent: "space-between",
                  alignItems: "center",
                  padding: "10px",
                }}
              >
                <div>
                  <img
                    src={r.img}
                    alt="profile"
                    style={{ width: "30px", height: "30px" }}
                  />
                  <b className="ms-2">{r.name}</b>
                </div>
                <div>
                  <button
                    className="me-2 btn btn-primary"
                    onClick={() => navigate(`/recipeDetails/${r.recipeId}`)}
                  >
                    View
                  </button>
                  <button
                    className="me-2 btn btn-danger"
                    onClick={() => deleteRecipe(r.recipeId)}
                  >
                    {recipeDeleteLoading ? (
                      <Spinner style={{ width: "1rem", height: "1rem" }} />
                    ) : (
                      "Delete"
                    )}
                  </button>
                  <button
                    className="me-2 btn btn-success"
                    onClick={() => allowRecipe(r.recipeId)}
                  >
                    {recipeAllowLoading ? (
                      <Spinner style={{ width: "1rem", height: "1rem" }} />
                    ) : (
                      "Allow"
                    )}
                  </button>
                </div>
              </div>
            ))}
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
