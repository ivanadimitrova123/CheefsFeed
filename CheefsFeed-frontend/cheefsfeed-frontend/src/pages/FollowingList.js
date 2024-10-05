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
      console.error("Error unfollowing user:", error);
    }
  };

  return (
    <div className="customBackground container-fluid">
      <Navbar />
      <div className="container mt-5 ">
        <h2>Your Following List</h2>
        {loading ? (
          <p>Loading...</p>
        ) : followingUsers.length > 0 ? (
          <ul className="followingList mt-5">
            {followingUsers.map((user) => (
              <li
                key={user.id}
                className="d-flex align-items-center justify-content-between mt-3"
                style={{ borderBottom: "1px solid #e0e0e0", paddingBottom: "10px" }}
              >
                <div className="d-flex align-items-center">
                  <img
                    src={
                      !user.picture ||
                      user.picture === `${baseUrl}/default.jpg`
                        ? `${baseUrl}/default.jpg`
                        : user.picture
                    }
                    alt={user.username}
                    style={{ width: "60px", height: "50px", marginRight: "15px" }}
                  />
                  <h3 className="mb-0">{user.username}</h3>
                </div>
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
