import React, { useState,useContext } from 'react';
import axios from '../axios/axios'; 
import Navbar from "../components/navbar";
import { getHeaders } from "../utils";
import { Store } from "../Store";

const AddCategory = () => {
  const [categoryName, setCategoryName] = useState('');
  const [error, setError] = useState(null);
  const { state } = useContext(Store);
  const { userInfo } = state;
  const [successMessage, setSuccessMessage] = useState('');

  const handleInputChange = (e) => {
    setCategoryName(e.target.value);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!categoryName) {
      setError('Category name is required');
      return;
    }

    const category = {
        name: categoryName
    };

    try {
    const headers = getHeaders(userInfo.token, false);

      const response = await axios.post('/category/addCategory', category, { headers });
      console.log('Category added:', response.data);
      setSuccessMessage('Category added successfully');
      setError(null);
      setCategoryName('');
    } catch (err) {
      console.error('Error adding category:', err);
      setError('Failed to add category. Please try again.');
    }
  };

  return (
    <div className="add-category">
    <Navbar/>
    <div className='container'>
        <h2>Add New Category</h2>
        <form onSubmit={handleSubmit}>
            <div className="form-group">
            <h4 htmlFor="categoryName" className='my-4'>Category Name:</h4>
            <input
                type="text"
                id="categoryName"
                value={categoryName}
                onChange={handleInputChange}
                placeholder="Enter category name"
                className="form-control mb-4"
            />
            </div>
            {error && <p className="error-message">{error}</p>}
            {successMessage && <p className="success-message">{successMessage}</p>}
            <button type="submit" className="btn fw-bold" 
                style={{
                    backgroundColor: "#fce7e3",
                    color: "#fb2c2c", 
                    borderColor: "#dea4a4",
                    borderRadius: "5px", 
                    padding: "0.5rem 1rem", 
                }}>
            Add Category
            </button>
        </form>
    </div>
      
    </div>
  );
};

export default AddCategory;
