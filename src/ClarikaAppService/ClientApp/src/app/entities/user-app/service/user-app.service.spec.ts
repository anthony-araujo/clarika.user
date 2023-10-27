import { TestBed } from "@angular/core/testing";
import {
  HttpClientTestingModule,
  HttpTestingController,
} from "@angular/common/http/testing";
import dayjs from "dayjs/esm";

import { DATE_FORMAT } from "app/config/input.constants";
import { IUserApp, UserApp } from "../user-app.model";

import { UserAppService } from "./user-app.service";

describe("UserApp Service", () => {
  let service: UserAppService;
  let httpMock: HttpTestingController;
  let elemDefault: IUserApp;
  let expectedResult: IUserApp | IUserApp[] | boolean | null;
  let currentDate: dayjs.Dayjs;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
    });
    expectedResult = null;
    service = TestBed.inject(UserAppService);
    httpMock = TestBed.inject(HttpTestingController);
    currentDate = dayjs();

    elemDefault = {
      id: 0,
      firstName: "AAAAAAA",
      lastName: "AAAAAAA",
      email: "AAAAAAA",
      dateBirth: currentDate,
      age: 0,
      passwordHash: "AAAAAAA",
      securityStamp: "AAAAAAA",
      concurrencyStamp: "AAAAAAA",
    };
  });

  describe("Service methods", () => {
    it("should find an element", () => {
      const returnedFromService = Object.assign(
        {
          dateBirth: currentDate.format(DATE_FORMAT),
        },
        elemDefault
      );

      service.find(123).subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "GET" });
      req.flush(returnedFromService);
      expect(expectedResult).toMatchObject(elemDefault);
    });

    it("should create a UserApp", () => {
      const returnedFromService = Object.assign(
        {
          id: 0,
          dateBirth: currentDate.format(DATE_FORMAT),
        },
        elemDefault
      );

      const expected = Object.assign(
        {
          dateBirth: currentDate,
        },
        returnedFromService
      );

      service
        .create(new UserApp())
        .subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "POST" });
      req.flush(returnedFromService);
      expect(expectedResult).toMatchObject(expected);
    });

    it("should update a UserApp", () => {
      const returnedFromService = Object.assign(
        {
          id: 1,
          firstName: "BBBBBB",
          lastName: "BBBBBB",
          email: "BBBBBB",
          dateBirth: currentDate.format(DATE_FORMAT),
          age: 1,
          passwordHash: "BBBBBB",
          securityStamp: "BBBBBB",
          concurrencyStamp: "BBBBBB",
        },
        elemDefault
      );

      const expected = Object.assign(
        {
          dateBirth: currentDate,
        },
        returnedFromService
      );

      service
        .update(expected)
        .subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "PUT" });
      req.flush(returnedFromService);
      expect(expectedResult).toMatchObject(expected);
    });

    it("should partial update a UserApp", () => {
      const patchObject = Object.assign(
        {
          firstName: "BBBBBB",
          lastName: "BBBBBB",
          age: 1,
          securityStamp: "BBBBBB",
          concurrencyStamp: "BBBBBB",
        },
        new UserApp()
      );

      const returnedFromService = Object.assign(patchObject, elemDefault);

      const expected = Object.assign(
        {
          dateBirth: currentDate,
        },
        returnedFromService
      );

      service
        .partialUpdate(patchObject)
        .subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "PATCH" });
      req.flush(returnedFromService);
      expect(expectedResult).toMatchObject(expected);
    });

    it("should return a list of UserApp", () => {
      const returnedFromService = Object.assign(
        {
          id: 1,
          firstName: "BBBBBB",
          lastName: "BBBBBB",
          email: "BBBBBB",
          dateBirth: currentDate.format(DATE_FORMAT),
          age: 1,
          passwordHash: "BBBBBB",
          securityStamp: "BBBBBB",
          concurrencyStamp: "BBBBBB",
        },
        elemDefault
      );

      const expected = Object.assign(
        {
          dateBirth: currentDate,
        },
        returnedFromService
      );

      service.query().subscribe((resp) => (expectedResult = resp.body));

      const req = httpMock.expectOne({ method: "GET" });
      req.flush([returnedFromService]);
      httpMock.verify();
      expect(expectedResult).toContainEqual(expected);
    });

    it("should delete a UserApp", () => {
      service.delete(123).subscribe((resp) => (expectedResult = resp.ok));

      const req = httpMock.expectOne({ method: "DELETE" });
      req.flush({ status: 200 });
      expect(expectedResult);
    });

    describe("addUserAppToCollectionIfMissing", () => {
      it("should add a UserApp to an empty array", () => {
        const userApp: IUserApp = { id: 123 };
        expectedResult = service.addUserAppToCollectionIfMissing([], userApp);
        expect(expectedResult).toHaveLength(1);
        expect(expectedResult).toContain(userApp);
      });

      it("should not add a UserApp to an array that contains it", () => {
        const userApp: IUserApp = { id: 123 };
        const userAppCollection: IUserApp[] = [
          {
            ...userApp,
          },
          { id: 456 },
        ];
        expectedResult = service.addUserAppToCollectionIfMissing(
          userAppCollection,
          userApp
        );
        expect(expectedResult).toHaveLength(2);
      });

      it("should add a UserApp to an array that doesn't contain it", () => {
        const userApp: IUserApp = { id: 123 };
        const userAppCollection: IUserApp[] = [{ id: 456 }];
        expectedResult = service.addUserAppToCollectionIfMissing(
          userAppCollection,
          userApp
        );
        expect(expectedResult).toHaveLength(2);
        expect(expectedResult).toContain(userApp);
      });

      it("should add only unique UserApp to an array", () => {
        const userAppArray: IUserApp[] = [
          { id: 123 },
          { id: 456 },
          { id: 4302 },
        ];
        const userAppCollection: IUserApp[] = [{ id: 123 }];
        expectedResult = service.addUserAppToCollectionIfMissing(
          userAppCollection,
          ...userAppArray
        );
        expect(expectedResult).toHaveLength(3);
      });

      it("should accept varargs", () => {
        const userApp: IUserApp = { id: 123 };
        const userApp2: IUserApp = { id: 456 };
        expectedResult = service.addUserAppToCollectionIfMissing(
          [],
          userApp,
          userApp2
        );
        expect(expectedResult).toHaveLength(2);
        expect(expectedResult).toContain(userApp);
        expect(expectedResult).toContain(userApp2);
      });

      it("should accept null and undefined values", () => {
        const userApp: IUserApp = { id: 123 };
        expectedResult = service.addUserAppToCollectionIfMissing(
          [],
          null,
          userApp,
          undefined
        );
        expect(expectedResult).toHaveLength(1);
        expect(expectedResult).toContain(userApp);
      });

      it("should return initial array if no UserApp is added", () => {
        const userAppCollection: IUserApp[] = [{ id: 123 }];
        expectedResult = service.addUserAppToCollectionIfMissing(
          userAppCollection,
          undefined,
          null
        );
        expect(expectedResult).toEqual(userAppCollection);
      });
    });
  });

  afterEach(() => {
    httpMock.verify();
  });
});
