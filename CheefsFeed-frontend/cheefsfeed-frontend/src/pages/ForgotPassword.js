import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import Navbar from '../components/navbar';

const ForgotPassword = () => {
    const [email, setEmail] = useState('');
    const [isSubmitted, setIsSubmitted] = useState(false);

    const handleInputChange = (e) => {
        setEmail(e.target.value);
    };

    const handleSubmit = (e) => {
        e.preventDefault();

        // Perform the logic to handle the forgot password submission
        // For this example, let's just log the email
        console.log('Forgot Password submitted for email:', email);

        // Update state to indicate that the form has been submitted
        setIsSubmitted(true);
    };

    return (
        <div className="container-fluid loginPageBg">
            <Navbar />
            <div className='container loginForm'>
                <h2 className="mt-4 mb-4 text-center"><b>Forgot Password</b></h2>
                {isSubmitted ? (
                    <p>An email with instructions to reset your password has been sent to {email}.</p>
                ) : (
                    <form onSubmit={handleSubmit}>
                        <div className="mb-3">
                            <label htmlFor="email" className="form-label">
                            <b>Enter Your Email Here:</b>
                            </label>
                            <input
                                type="email"
                                id="email"
                                name="email"
                                value={email}
                                onChange={handleInputChange}
                                className="form-control"
                                required
                            />
                        </div>
                        <div className='buttonSide'>
                            <button type="submit" className="btn btn-primary rounded-pill">
                                Submit
                            </button>
                        </div>
                    </form>
                )}
                <p className='mt-3 text-center'>Back to <Link to="/login"><b>Log In?</b></Link></p>
            </div>
        </div>
    );
};

export default ForgotPassword;