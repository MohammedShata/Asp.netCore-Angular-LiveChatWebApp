
import { Photo } from "./photo";

export interface Member {
  id: number;
  age: number;
  userName: string;
  knownAs: string;
  passwordHash: string;
  passwordSalt: string;
  dateOfBirth: Date;
  lastActive: Date;
  gender: string;
  introduction: string;
  lookingFor: string;
  interests: string;
  city: string;
  country: string;
  photoUrl: string;
  created:Date;
  photos: Photo[];
}

