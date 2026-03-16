import React, { useState, useEffect, useRef } from 'react';

const FormComponent = ({ fields, initialValues = {}, onSubmit }) => {
  const [formValues, setFormValues] = useState(initialValues);
  const [errors, setErrors] = useState({});
  const initialValuesSet = useRef(false);

  useEffect(() => {
    if (!initialValuesSet.current) {
      setFormValues(initialValues);
      initialValuesSet.current = true;
    }
  }, [initialValues]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormValues({
      ...formValues,
      [name]: value,
    });
  };

  const validate = () => {
    const newErrors = {};
    Object.keys(fields).forEach((field) => {
      const value = formValues[field];
      const validation = fields[field].validation;
      if (validation) {
        if (validation.required && !value) {
          newErrors[field] = `${fields[field].label} is required`;
        }
        if (validation.maxLength && value && value.length > validation.maxLength) {
          newErrors[field] = `${fields[field].label} must be at most ${validation.maxLength} characters`;
        }
        if (validation.isEmail && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value)) {
          newErrors[field] = `${fields[field].label} must be a valid email address`;
        }
        if (validation.pattern && !validation.pattern.test(value)) {
          newErrors[field] = `${fields[field].label} is invalid`;
        }
        if (validation.minLength && value &&value.length < validation.minLength) {
          newErrors[field] = `${fields[field].label} must be at least ${validation.minLength} characters`;
        }
        if (validation.min !== undefined && Number(value) < validation.min) {
          newErrors[field] = `${fields[field].label} must be at least ${validation.min}`;
        }
        if (validation.max !== undefined && Number(value) > validation.max) {
          newErrors[field] = `${fields[field].label} must be at most ${validation.max}`;
        }
        if (validation.isPassword) {
          const hasAlphabetic = /[a-zA-Z]/.test(value);
          const hasNumeric = /[0-9]/.test(value);
          const hasSpecialChar = /[!@#$%^&*(),.?":{}|<>]/.test(value);
          const hasUppercase = /[A-Z]/.test(value);

          if (!hasAlphabetic) {
            newErrors[field] = `${fields[field].label} must contain at least one alphabetic character`;
          } else if (!hasNumeric) {
            newErrors[field] = `${fields[field].label} must contain at least one numeric character`;
          } else if (!hasSpecialChar) {
            newErrors[field] = `${fields[field].label} must contain at least one special character`;
          } else if (!hasUppercase) {
            newErrors[field] = `${fields[field].label} must contain at least one uppercase letter`;
          }
        }
      }
    });
    return newErrors;
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    const validationErrors = validate();
    if (Object.keys(validationErrors).length > 0) {
      setErrors(validationErrors);
    } else {
      setErrors({});
      onSubmit(formValues);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      {Object.keys(fields).map((field) => (
        <div key={field}>
          <label htmlFor={field}>{fields[field].label}</label>
          <input
            id={field}
            type={fields[field].type}
            name={field}
            value={formValues[field] || ''}
            onChange={handleChange}
          />
          {errors[field] && <span style={{ color: 'red' }}>{errors[field]}</span>}
        </div>
      ))}
      <button type="submit">Submit</button>
    </form>
  );
};

export default FormComponent;
