import { Fragment, useEffect, useRef, useState } from "react";
import Title from "@/components/ui/title";
import { Button, Container, Input, Spacer } from "@nextui-org/react";
import DropdownCharacterType from "@/components/dropdown-character-type/DropdownCharacterType";
import DropdownAlignment from "@/components/dropdown-alignment/DropdownAlignment";
import { createNewRole, getAllRoles } from "../../../../data/back-api";
import { Text } from "@nextui-org/react";
import classes from "../index.module.css";
import { Check, XOctagon } from "react-feather";
import { toLowerRemoveDiacritics } from "@/helper/string";

export default function CreateRole() {
  const [roleName, setRoleName] = useState("");
  const [characterType, setCharacterType] = useState();
  const [alignment, setAlignment] = useState();
  const [message, setMessage] = useState(<Fragment />);

  const [resetCharacterType, setResetCharacterType] = useState("characterType");
  const [resetAlignment, setResetAlignment] = useState("alignment");

  const [roles, setRoles] = useState<string[]>([]);
  useEffect(() => {
    async function initRoles() {
      const tempRoles = (await getAllRoles()).map((role) => {
        return role.name;
      });
      setRoles(tempRoles);
    }
    initRoles();
  }, []);

  const title = <Title>Création d{"'"}un nouveau rôle</Title>;

  async function createRole() {
    if (characterType === undefined || alignment === undefined) return;

    if (await createNewRole(roleName, characterType, alignment)) {
      roles.push(roleName);
      setRoleName("");
      setRoles(roles);
      setResetCharacterType(resetCharacterType + " ");
      setResetAlignment(resetAlignment + " ");

      updateMessage(false, `Rôle "${roleName}" enregistré correctement.`);
    } else {
      //Erreur
      updateMessage(
        true,
        "Une erreur est survenue lors de l'enregistrement du rôle."
      );
    }
  }

  function updateMessage(isError: boolean, message: string) {
    if (isError) {
      setMessage(
        <Text span className={classes.red}>
          <XOctagon className={classes.icon} />
          {message}
        </Text>
      );
    } else {
      setMessage(
        <Text span className={classes.green}>
          <Check className={classes.icon} />
          {message}
        </Text>
      );
    }
  }

  function canCreateRole() {
    // Can create a role when
    //  - all params are set
    //  - the name is unique
    return (
      roleName !== "" &&
      roles.filter(
        (p) => toLowerRemoveDiacritics(p) === toLowerRemoveDiacritics(roleName)
      ).length === 0 &&
      characterType !== undefined &&
      alignment !== undefined
    );
  }

  function roleNameChanged(value: string) {
    setRoleName(value);
    checkError(value);
  }

  function checkError(roleName: string) {
    setMessage(<Fragment />);

    if (roleName === "") {
      updateMessage(true, "Le nom du rôle ne doit pas être vide.");
      return;
    }

    if (
      roles.filter(
        (p) => toLowerRemoveDiacritics(p) === toLowerRemoveDiacritics(roleName)
      ).length !== 0
    ) {
      updateMessage(true, `Le rôle '${roleName}' existe déjà.`);
      return;
    }
  }

  return (
    <Fragment>
      {title}
      <Spacer y={2} />
      {message}
      <Spacer y={2} />
      <Container fluid css={{ display: "flex", flexDirection: "column" }}>
        <Input
          clearable
          bordered
          labelPlaceholder="Nom"
          aria-label="Nom"
          onChange={(event) => roleNameChanged(event.target.value)}
        ></Input>
        <Spacer y={1.75} />
        <DropdownCharacterType
          key={resetCharacterType}
          setCharacterType={setCharacterType}
        />
        <Spacer y={1.75} />
        <DropdownAlignment key={resetAlignment} setAlignment={setAlignment} />
        <Spacer y={3} />
      </Container>

      <Button
        shadow
        ghost
        color="success"
        onPress={createRole}
        disabled={!canCreateRole()}
      >
        Créer un rôle
      </Button>
      <Spacer y={3} />
    </Fragment>
  );
}
