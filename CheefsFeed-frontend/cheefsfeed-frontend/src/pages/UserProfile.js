import React, { useState, useEffect, useContext } from "react";
import { Link, useParams, useNavigate } from "react-router-dom";
import axios from "../axios/axios";
import Navbar from "../components/navbar";
import { Store } from "../Store";
import FeedItem from "../components/FeedItem";

function UserProfile() {
  const { state } = useContext(Store);
  const { userInfo } = state;
  const { id } = useParams();
  const navigate = useNavigate();
  const [user, setUser] = useState("");
  const [isFollowing, setIsFollowing] = useState(false);
  const baseUrl = window.location.origin;
  const [followLoading, setFollowLoading] = useState(false);

  useEffect(() => {
    const headers = {
      Authorization: `Bearer ${userInfo.token}`,
    };

    const checkFollowStatus = async () => {
      try {
        const response = await axios.get(`follow/status/${id}`, { headers });
        setIsFollowing(response.data);
      } catch (error) {
        console.error("Network error:", error);
      }
    };

    if (id) {
      axios
        .get(`account/user/${id}`, { headers })
        .then((response) => {
          setUser(response.data);
          if (id !== userInfo.user.id) {
            checkFollowStatus();
          }
        })
        .catch((error) => {
          console.error("Error fetching user data:", error);
        });
    }
  }, [userInfo, id]);

  const handleFollowing = () => {
    navigate(`/followingList`);
  };

  const handleFollow = async () => {
    setFollowLoading(true);
    const headers = {
      Authorization: `Bearer ${userInfo.token}`,
    };

    try {
      await axios.post(`follow/${user.id}`, null, { headers });
      setIsFollowing(true);
    } catch (error) {
      console.error("Failed to follow this user:", error);
    } finally {
      setFollowLoading(false);
    }
  };

  const handleUnfollow = async () => {
    setFollowLoading(true);
    const headers = {
      Authorization: `Bearer ${userInfo.token}`,
    };

    try {
      await axios.delete(`follow/${user.id}`, { headers });
      setIsFollowing(false);
    } catch (error) {
      console.error("Error unfollowing user:", error);
    } finally {
      setFollowLoading(false);
    }
  };

  return (
    <div className="heightMore">
      <div className="container-fluid customBackground">
        <Navbar />
        <div className="container pt-5 userProfileContainer">
          <div className="row">
            <div className="col-4 leftPartProfile">
              <div className="centeredProfile">
                {!user ? (
                  <div className="text-center">
                    <p>Loading...</p>
                    <div className="spinner-border" role="status">
                      <span className="visually-hidden">Loading...</span>
                    </div>
                  </div>
                ) : (
                  <>
                    <img
                      src={
                        user.userImage
                          ? user.userImage
                          : `${baseUrl}/default.jpg`
                      }
                      alt="Profile"
                      className="img-fluid rounded-circle"
                    />
                    <div>
                      <h2>{user.username}</h2>
                      {user.id !== userInfo.user.id && (
                        <div className="theBtns">
                          {isFollowing ? (
                            <button
                              onClick={handleUnfollow}
                              className="btn btn-danger unfollowBtn"
                            >
                              {followLoading ? (
                                <div
                                  className="spinner-border"
                                  role="status"
                                  style={{ width: "1rem", height: "1rem" }}
                                >
                                  <span className="visually-hidden"></span>
                                </div>
                              ) : (
                                "Unfollow"
                              )}
                            </button>
                          ) : (
                            <button
                              onClick={handleFollow}
                              className="btn btn-primary followBtn"
                            >
                              {followLoading ? (
                                <div
                                  className="spinner-border"
                                  role="status"
                                  style={{ width: "1rem", height: "1rem" }}
                                >
                                  <span className="visually-hidden"></span>
                                </div>
                              ) : (
                                "Follow"
                              )}
                            </button>
                          )}
                        </div>
                      )}
                    </div>
                  </>
                )}
              </div>
              {user && userInfo.user.id === user.id && (
                <Link to="/addProfilePicture" className=" mt-4">
                  Change Profile Picture
                </Link>
              )}
            </div>
            <div className="col-6">
              {user && (
                <div className="profileInfo">
                  <p>
                    <b>{user.recipes ? user.recipes.length : 0}</b> posts
                  </p>
                 
                  <p>
                    <b>
                        {user.following}
                    </b> 
                    following
                  </p>
                  <p>
                    <b>{user.followers}</b> followers
                  </p>
                  {user && userInfo.user.id === user.id && (
                    <div className="theBtns">
                      <Link to="/recipeForm" className="btn btn-danger">
                        <b>Add Recipe</b>
                      </Link>
                      <button
                        className="btn btn-primary "
                        onClick={handleFollowing}
                      >
                        <b>Following List</b>
                      </button>
                    </div>
                  )}
                </div>
              )}
            </div>
          </div>
        </div>

        <div className="container mt-4 cardGroup">
          <h2 className="text-start">
            <b>Posts</b>
          </h2>
          {user && user.id === userInfo.user.id && user.recipes.length <= 0 && (
            <h4 className="noPostText">
              Make your first{" "}
              <span>
                <Link to="/recipeForm">post!</Link>
              </span>
            </h4>
          )}

          {user.recipes && user.recipes.length > 0 && (
            <div className="row mt-4 paddingBottom">
              {user.recipes.map((recipe) => (
                <FeedItem recipe={recipe} user={user} key={recipe.id} />
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  );
}

export default UserProfile;