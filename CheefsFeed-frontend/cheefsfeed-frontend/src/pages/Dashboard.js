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
  const [commentRefresh, setCommentRefresh] = useState(false);
  const [recipeRefresh, setRecipeRefresh] = useState(false);
  const [commentDeleteLoading, setCommentDeleteLoading] = useState(false);
  const [recipeDeleteLoading, setRecipeDeleteLoading] = useState(false);
  const [commentAllowLoading, setCommentAllowLoading] = useState(false);
  const [recipeAllowLoading, setRecipeAllowLoading] = useState(false);

  const fetchUserData = async (userId) => {
    const headers = getHeaders(userInfo.token, false);
    try {
      const response = await axios.get(`/account/user/${userId}`, { headers });
      return response.data; 
    } catch (error) {
      console.error(`Error fetching user with ID ${userId}:`, error);
      return null; 
    }
  };

  const fetchReportedComments = async () => {
    setCommentsIsLoading(true);
    const headers = getHeaders(userInfo.token, false);
    try {
      const response = await axios.get("/reportcomment", { headers });
      const commentsWithUserData = await Promise.all(
        response.data.map(async (c) => {
          const user = await fetchUserData(c.comment.userId);
          return {
            ...c,
            user, 
          };
        })
      );
      setComments(commentsWithUserData);
    } catch (error) {
      console.error("Error fetching reported comments:", error);
    } finally {
      setCommentsIsLoading(false);
    }
  };

  const fetchReportedRecipes = async () => {
    setRecipesIsLoading(true);
    const headers = getHeaders(userInfo.token, false);
    try {
      const response = await axios.get("/reportedrecipe", { headers });
      setRecipes(response.data);
    } catch (error) {
      console.error("Error fetching reported recipes:", error);
    } finally {
      setRecipesIsLoading(false);
    }
  };

  const deleteHandler = async (id) => {
    const headers = getHeaders(userInfo.token, false);
    setCommentDeleteLoading(true);
    try {
      await axios.delete(`/reportcomment/delete/${id}`, { headers });
      setCommentRefresh(true);
    } catch (error) {
      console.error("Error deleting comment:", error);
    } finally {
      setCommentDeleteLoading(false);
    }
  };

  const allowComment = async (id) => {
    const headers = getHeaders(userInfo.token, false);
    setCommentAllowLoading(true);
    try {
      await axios.delete(`/reportcomment/${id}`, {}, { headers });
      setCommentRefresh(true); 
    } catch (error) {
      console.error("Error allowing comment:", error);
    } finally {
      setCommentAllowLoading(false);
    }
  };

  const deleteRecipe = async (recipeId) => {
    const headers = getHeaders(userInfo.token, false);
    setRecipeDeleteLoading(true);
    try {
      await axios.delete(`/reportedrecipe/delete/${recipeId}`, { headers });
      setRecipeRefresh(true);
    } catch (error) {
      console.error("Error deleting recipe:", error);
    } finally {
      setRecipeDeleteLoading(false);
    }
  };

  const allowRecipe = async (recipeId) => {
    const headers = getHeaders(userInfo.token, false);
    setRecipeAllowLoading(true);
    try {
      await axios.delete(`/reportedrecipe/${recipeId}`, {}, { headers });
      setRecipeRefresh(true);
    } catch (error) {
      console.error("Error allowing recipe:", error);
    } finally {
      setRecipeAllowLoading(false);
    }
  };

  useEffect(() => {
    fetchReportedComments();
    fetchReportedRecipes();
    if (commentRefresh) setCommentRefresh(false);
    if (recipeRefresh) setRecipeRefresh(false);
  }, [commentRefresh, recipeRefresh, userInfo]);

  return (
    <div className="container-fluid customBg" style={{ minHeight: "100vh" }}>
      <Navbar />
      <div className="row mb-3">
        <div className="col" style={{ display: "flex", alignItems: "center" }}>
          <h2 className="mainHeader" style={{ width: "100%", textAlign: "center" }}>
            Admin Dashboard
          </h2>
        </div>
      </div>
      <div className="mt-5" style={{ display: "flex", justifyContent: "space-evenly" }}>
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
          {comments.map((c) => (
            <div className="mt-3" key={c.commentId} style={{ border: "1px solid black", padding: "10px" }}>
              <div className="row">
                <div className="col-12 col-xxl-8">
                  <img
                    src={c.user.userImage || "default_picture_url"}
                    alt="profile"
                    style={{ width: "30px", height: "30px" }}
                  />
                  <b className="ms-2">{c.user.username}</b>
                  <span className="ms-3">{c.comment.content}</span>
                </div>
                <div className="col-12 col-xxl-4 mt-3 mt-xxl-0">
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
          {recipes.map((r) => (
            <div className="mt-3" key={r.recipeId} style={{ border: "1px solid black", padding: "10px" }}>
              <div className="row">
                <div className="col-12 col-xxl-8">
                  <img
                    src={r.img || "default_recipe_image_url"}
                    alt="recipe"
                    style={{ width: "30px", height: "30px" }}
                  />
                  <b className="ms-2">{r.name}</b>
                </div>
                <div className="col-12 col-xxl-4 mt-3 mt-xxl-0">
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
            </div>
          ))}
        </div>
      </div>
    </div>
  );
};

export default Dashboard;
