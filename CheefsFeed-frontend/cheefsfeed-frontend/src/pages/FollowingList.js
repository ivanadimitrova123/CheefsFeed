import React, { useState, useEffect, useContext } from "react";
import Navbar from "../components/navbar";
import { Store } from "../Store";
import axios from "../axios/axios";

function FollowingList() {
  const baseUrl = window.location.origin;
  const { state } = useContext(Store);
  const { userInfo } = state;
  const [followingUsers, setFollowingUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [refresh, setRefresh] = useState(false);

  useEffect(() => {
    const fetchFollowingUsers = async () => {
      try {
        const response = await axios.get("follow/following", {
            headers: {
              Authorization: `Bearer ${userInfo.token}`,
            },
          });
        setFollowingUsers(response.data);
        setLoading(false);
      } catch (error) {
        console.error("Error fetching following users:", error);
      }
    };
    fetchFollowingUsers();
    if (refresh === true) {
      setRefresh(false);
    }
  }, [userInfo, refresh]);

  const handleUnfollow = async (followedUserId) => {
    try {
        await axios.delete(`follow/${followedUserId}`, {
            headers: {
              Authorization: `Bearer ${userInfo.token}`,
            },
        });
      setRefresh(true);
    } catch (error) {
     // setUnfollowLoading(false);
      console.error("Error unfollowing user:", error);
    }
  };

  return (
    <div>
      <Navbar />
      <div className="container mt-5">
      <h2>Your Following List</h2>
      {loading ? (
        <p>Loading...</p>
      ) : followingUsers.length > 0 ? (
        <ul
          className="followingList mt-3"
          style={{
            display: "flex",
            flexDirection: "column",
            justifyContent: "center",
          }}
        >
          {followingUsers.map((user) => (
            <li key={user.id} className="mt-3">
              <img
                src={
                    !user.picture ||
                    user.picture === `${baseUrl}/default.jpg`
                      ? `${baseUrl}/default.jpg`
                      : user.picture
                  }
                alt={user.username}
                style={{ width: "50px", height: "50px" }}
              />
              <h3>{user.username}</h3>

              <button
                className="btn btn-danger"
                onClick={() => handleUnfollow(user.id)}
              >
                Unfollow
              </button>
            </li>
          ))}
        </ul>
      ) : (
        <p className="mt-3 fs-5">You are not following any users.</p>
      )}
      </div>
    </div>
  );
}

export default FollowingList;