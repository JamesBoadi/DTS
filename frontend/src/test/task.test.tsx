/*import { fireEvent, render, waitFor } from "@testing-library/react";
import { CreateCardProps, ProfileDataResponse, CardType } from "../interface/interface";

import { fetchData } from '../api/api';


const mockProfileData: ProfileDataResponse = {
  id: 1,
  name: "John",
  socialMediaHandle: "@ohnDoe",
  profileImgSrc: "...",
  bio: "...",
  location: "asgard",
  website: "https://janesmith.com"
}

const cardType: CardType = { cardType: "horizontal" };

const url = "http://localhost:3000/user/1";

global.fetch = jest.fn(() =>
  Promise.resolve({
    json: () => Promise.resolve(mockProfileData),
  }),
) as jest.Mock;

describe('Test if card renders correctly', () => {
  test("If ParentComponent is not passed open, ChildComponent is not called", async () => {
    const setState = jest.fn();
    const { getByText } = render(<Card cardType={cardType} />);

    await fetch(url).then((data) => {
      setState({
        loadedProfileData: true,
        profile_data: data
      });
    });

    await waitFor(() => {
      expect(setState).toBeCalled();
      
      const viewCVButton = getByText("View CV");
      fireEvent.click(viewCVButton);

      // .... Additional logic .....
    });

  });
});*/

export {}