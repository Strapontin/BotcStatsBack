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
import { Alignment } from "@/entities/enums/alignment";

export default function CreateRole() {
  const [inputKey, setInputKey] = useState(0);
  const [roleName, setRoleName] = useState("");
  const [characterType, setCharacterType] = useState();
  const [alignment, setAlignment] = useState(Alignment.None);
  const [message, setMessage] = useState(<Fragment />);

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

      updateMessage(false, `Rôle "${roleName}" enregistré correctement.`);
      setInputKey(inputKey + 1);
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

    const roleWithSameName = roles.filter(
      (p) => toLowerRemoveDiacritics(p) === toLowerRemoveDiacritics(roleName)
    );
    if (roleWithSameName.length !== 0) {
      updateMessage(true, `Le rôle '${roleWithSameName[0]}' existe déjà.`);
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
          key={inputKey}
          clearable
          bordered
          labelPlaceholder="Nom"
          aria-label="Nom"
          onChange={(event) => roleNameChanged(event.target.value)}
        />
        <Spacer y={1.75} />
        <DropdownCharacterType
          key={inputKey + 1}
          setCharacterType={setCharacterType}
        />
        <Spacer y={1.75} />
        <DropdownAlignment
          key={inputKey + 2}
          setAlignment={setAlignment}
          alignment={alignment}
          defaultText="Alignement"
        />
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
