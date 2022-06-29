import React, { useState, useContext, useEffect } from 'react';
import { GlobalContext } from "../context/GlobalState";
import { Link, useHistory } from "react-router-dom";
import {
  Form,
  FormGroup,
  Label,
  Input,
  Button
} from "reactstrap";

export const EditHotel = (props) => {
  const { editHotel, Hotels } = useContext(GlobalContext);
  const [selectedHotel, setSelectedHotel] = useState({
    id: '',
    name: ''
  })
  const history = useHistory();
  const currentHotelId = props.match.params.id;

  useEffect(() => {
    const HotelId = currentHotelId;
    const selectedHotel = Hotels.find(Hotel => Hotel.id === HotelId);
    setSelectedHotel(selectedHotel);
  }, [currentHotelId, Hotels])

  const onChange = (e) => {
    setSelectedHotel({ ...selectedHotel, [e.target.name]: e.target.value })
  }

  const onSubmit = (e) => {
    e.preventDefault();
    editHotel(selectedHotel);
    history.push("/")
  }

  return (
    <Form onSubmit={onSubmit}>
      <FormGroup>
        <Label>Name</Label>
        <Input type="text" value={selectedHotel.name} onChange={onChange} name="name" placeholder="Enter Hotel" required></Input>
      </FormGroup>
      <Button type="submit">Edit Name</Button>
      <Link to="/" className="btn btn-danger ml-2">Cancel</Link>
    </Form>
  )
}