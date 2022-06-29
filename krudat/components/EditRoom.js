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

export const EditRoom = (props) => {
  const { editRoom, Rooms } = useContext(GlobalContext);
  const [selectedRoom, setSelectedRoom] = useState({
    id: '',
    name: ''
  })
  const history = useHistory();
  const currentRoomId = props.match.params.id;

  useEffect(() => {
    const RoomId = currentRoomId;
    const selectedRoom = Rooms.find(Room => Room.id === RoomId);
    setSelectedRoom(selectedRoom);
  }, [currentRoomId, Rooms])

  const onChange = (e) => {
    setSelectedRoom({ ...selectedRoom, [e.target.name]: e.target.value })
  }

  const onSubmit = (e) => {
    e.preventDefault();
    editRoom(selectedRoom);
    history.push("/")
  }

  return (
    <Form onSubmit={onSubmit}>
      <FormGroup>
        <Label>Name</Label>
        <Input type="text" value={selectedRoom.name} onChange={onChange} name="name" placeholder="Enter Room" required></Input>
      </FormGroup>
      <Button type="submit">Edit Name</Button>
      <Link to="/" className="btn btn-danger ml-2">Cancel</Link>
    </Form>
  )
}