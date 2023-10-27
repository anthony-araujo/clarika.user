import { TestBed } from "@angular/core/testing";
import {
  HttpClientTestingModule,
  HttpTestingController,
} from "@angular/common/http/testing";

import { IUserLocation, UserLocation } from "../user-location.model";

import { UserLocationService } from "./user-location.service";

describe("UserLocation Service", () => {
  let service: UserLocationService;
  let httpMock: HttpTestingController;
  let elemDefault: IUserLocation;
  let expectedResult: IUserLocation | IUserLocation[] | boolean | null;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    expectedResult = null;
    service = TestBed.inject(UserLocationService);
    httpMock = TestBed.inject(HttpTestingController);

    elemDefault = {
      id: 0,
      address: "AAAAAAA",
      zipCode: "AAAAAAA",
      province: "AAAAAAA",
    };
  });

  describe("Service methods", () => {
    it("should find an element", () => {
      const returnedFromService = Object.assign({}, elemDefault);

      service.find(123).subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "GET" });
      req.flush(returnedFromService);
      expect(expectedResult).toMatchObject(elemDefault);
    });

    it("should create a UserLocation", () => {
      const returnedFromService = Object.assign(
        {
          id: 0,
        },
        elemDefault
      );

      const expected = Object.assign({}, returnedFromService);

      service
        .create(new UserLocation())
        .subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "POST" });
      req.flush(returnedFromService);
      expect(expectedResult).toMatchObject(expected);
    });

    it("should update a UserLocation", () => {
      const returnedFromService = Object.assign(
        {
          id: 1,
          address: "BBBBBB",
          zipCode: "BBBBBB",
          province: "BBBBBB",
        },
        elemDefault
      );

      const expected = Object.assign({}, returnedFromService);

      service
        .update(expected)
        .subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "PUT" });
      req.flush(returnedFromService);
      expect(expectedResult).toMatchObject(expected);
    });

    it("should partial update a UserLocation", () => {
      const patchObject = Object.assign(
        {
          province: "BBBBBB",
        },
        new UserLocation()
      );

      const returnedFromService = Object.assign(patchObject, elemDefault);

      const expected = Object.assign({}, returnedFromService);

      service
        .partialUpdate(patchObject)
        .subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "PATCH" });
      req.flush(returnedFromService);
      expect(expectedResult).toMatchObject(expected);
    });

    it("should return a list of UserLocation", () => {
      const returnedFromService = Object.assign(
        {
          id: 1,
          address: "BBBBBB",
          zipCode: "BBBBBB",
          province: "BBBBBB",
        },
        elemDefault
      );

      const expected = Object.assign({}, returnedFromService);

      service.query().subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "GET" });
      req.flush([returnedFromService]);
      httpMock.verify();
      expect(expectedResult).toContainEqual(expected);
    });

    it("should delete a UserLocation", () => {
      service.delete(123).subscribe((resp) => (expectedResult = resp.ok));

      const req = httpMock.expectOne({ method: "DELETE" });
      req.flush({ status: 200 });
      expect(expectedResult);
    });

    describe("addUserLocationToCollectionIfMissing", () => {
      it("should add a UserLocation to an empty array", () => {
        const userLocation: IUserLocation = { id: 123 };
        expectedResult = service.addUserLocationToCollectionIfMissing(
          [],
          userLocation
        );
        expect(expectedResult).toHaveLength(1);
        expect(expectedResult).toContain(userLocation);
      });

      it("should not add a UserLocation to an array that contains it", () => {
        const userLocation: IUserLocation = { id: 123 };
        const userLocationCollection: IUserLocation[] = [
          {
            ...userLocation,
          },
          { id: 456 },
        ];
        expectedResult = service.addUserLocationToCollectionIfMissing(
          userLocationCollection,
          userLocation
        );
        expect(expectedResult).toHaveLength(2);
      });

      it("should add a UserLocation to an array that doesn't contain it", () => {
        const userLocation: IUserLocation = { id: 123 };
        const userLocationCollection: IUserLocation[] = [{ id: 456 }];
        expectedResult = service.addUserLocationToCollectionIfMissing(
          userLocationCollection,
          userLocation
        );
        expect(expectedResult).toHaveLength(2);
        expect(expectedResult).toContain(userLocation);
      });

      it("should add only unique UserLocation to an array", () => {
        const userLocationArray: IUserLocation[] = [
          { id: 123 },
          { id: 456 },
          { id: 19847 },
        ];
        const userLocationCollection: IUserLocation[] = [{ id: 123 }];
        expectedResult = service.addUserLocationToCollectionIfMissing(
          userLocationCollection,
          ...userLocationArray
        );
        expect(expectedResult).toHaveLength(3);
      });

      it("should accept varargs", () => {
        const userLocation: IUserLocation = { id: 123 };
        const userLocation2: IUserLocation = { id: 456 };
        expectedResult = service.addUserLocationToCollectionIfMissing(
          [],
          userLocation,
          userLocation2
        );
        expect(expectedResult).toHaveLength(2);
        expect(expectedResult).toContain(userLocation);
        expect(expectedResult).toContain(userLocation2);
      });

      it("should accept null and undefined values", () => {
        const userLocation: IUserLocation = { id: 123 };
        expectedResult = service.addUserLocationToCollectionIfMissing(
          [],
          null,
          userLocation,
          undefined
        );
        expect(expectedResult).toHaveLength(1);
        expect(expectedResult).toContain(userLocation);
      });

      it("should return initial array if no UserLocation is added", () => {
        const userLocationCollection: IUserLocation[] = [{ id: 123 }];
        expectedResult = service.addUserLocationToCollectionIfMissing(
          userLocationCollection,
          undefined,
          null
        );
        expect(expectedResult).toEqual(userLocationCollection);
      });
    });
  });

  afterEach(() => {
    httpMock.verify();
  });
});
